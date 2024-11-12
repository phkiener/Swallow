namespace Swallow.Validation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Errors;

/// <summary>
///     A validation container able to handle validating any object.
/// </summary>
/// <remarks>
///     For a validation to happen, you need to register an <see cref="IAsserter{TValue}" /> first.
///     Having registered the asserter, you can call <see cref="Check" /> using any value of the correct type
///     and the asserter will be executed. You can also register multiple asserters for the same type; they will
///     be executed in the order they are registered.
///     When there is no matching asserter, the given value will pass validation.
///     Note that the type of the asserter and the type of the validated value must match exactly.
/// </remarks>
public sealed class ValidationContainer : IAsserter<object>
{
    private readonly IDictionary<Type, ICollection<GenericAsserter>> assertersByType;

    /// <summary>
    ///     Construct an empty validation container, possessing no asserters whatsoever.
    /// </summary>
    public ValidationContainer()
    {
        assertersByType = new Dictionary<Type, ICollection<GenericAsserter>>();
    }

    /// <summary>
    ///     Construct a validation container initialized with the given asserters.
    /// </summary>
    /// <param name="asserters">The asserters to register</param>
    /// <remarks>
    ///     This is just a short-hand way to call <see cref="Register" /> on many asserters.
    ///     The ctor will throw in the same circumstances as <see cref="Register" /> does.
    /// </remarks>
    public ValidationContainer(IEnumerable<object> asserters) : this()
    {
        foreach (var asserter in asserters)
        {
            Register(asserter);
        }
    }

    /// <summary>
    ///     Validate the given object using one or more of the registered asserters.
    /// </summary>
    /// <param name="valueProvider">Provider yielding the value to assert.</param>
    /// <param name="error">The resulting validation error or <c>null</c>.</param>
    /// <returns><c>True</c> if validation succeeded, <c>false</c> if a validation error happened.</returns>
    public bool Check(INamedValueProvider<object> valueProvider, out ValidationError error)
    {
        var assertedType = valueProvider.Value.GetType();
        foreach (var asserter in GetAsserters(assertedType))
        {
            if (asserter.Check(valueProvider: valueProvider, assertionType: assertedType, error: out error) is false)
            {
                return false;
            }
        }

        error = null;

        return true;
    }

    /// <summary>
    ///     Register an asserter for this validation context.
    /// </summary>
    /// <param name="asserter">The asserter to register.</param>
    /// <typeparam name="T">The type of value the asserter may validate.</typeparam>
    /// <returns>The validation container.</returns>
    public ValidationContainer Register<T>(IAsserter<T> asserter)
    {
        RegisterAsserter(assertedType: typeof(T), asserterType: typeof(IAsserter<T>), asserter: asserter);

        return this;
    }

    /// <summary>
    ///     Register an asserter for this validation context.
    /// </summary>
    /// <param name="asserter">The instance of the asserter to register.</param>
    /// <returns>The validation container.</returns>
    public ValidationContainer Register(object asserter)
    {
        var type = asserter.GetType();
        var asserterInterface = type.GetInterfaces().SingleOrDefault(i => i.Name == typeof(IAsserter<>).Name);
        if (asserterInterface == null)
        {
            throw new ArgumentException(message: $"Given asserter does not implement {typeof(IAsserter<>).Name}", paramName: nameof(asserter));
        }

        var assertedType = asserterInterface.GenericTypeArguments.Single();
        RegisterAsserter(assertedType: assertedType, asserterType: asserterInterface, asserter: asserter);

        return this;
    }

    private void RegisterAsserter(Type assertedType, Type asserterType, object asserter)
    {
        if (!assertersByType.ContainsKey(assertedType))
        {
            assertersByType.Add(key: assertedType, value: new List<GenericAsserter>());
        }

        var method = asserterType.GetMethod(nameof(IAsserter<object>.Check))
                     ?? throw new InvalidOperationException(
                         $"Asserter {asserterType.Name} does not have a method called {nameof(IAsserter<object>.Check)}");

        assertersByType[assertedType].Add(new(asserter: asserter, asserterMethod: method));
    }

    private IEnumerable<GenericAsserter> GetAsserters(Type type)
    {
        return assertersByType.TryGetValue(type, out var value) ? value : Array.Empty<GenericAsserter>();
    }

    private sealed class GenericAsserter
    {
        private readonly object asserter;
        private readonly MethodInfo asserterMethod;

        public GenericAsserter(object asserter, MethodInfo asserterMethod)
        {
            this.asserter = asserter;
            this.asserterMethod = asserterMethod;
        }

        public bool Check(INamedValueProvider<object> valueProvider, Type assertionType, out ValidationError error)
        {
            var typedNamedValueProvider = ConvertToTypedNamedValueProvider(rawNamedValueProvider: valueProvider, targetType: assertionType);
            var parameters = new[] { typedNamedValueProvider, null };
            var result = asserterMethod.Invoke(obj: asserter, parameters: parameters);
            error = parameters[1] as ValidationError;

            return (bool)result!;
        }

        private static object ConvertToTypedNamedValueProvider(INamedValueProvider<object> rawNamedValueProvider, Type targetType)
        {
            var valueProviderType = typeof(CastingNamedValueProvider<,>).MakeGenericType(targetType, typeof(object));

            return Activator.CreateInstance(type: valueProviderType, rawNamedValueProvider)
                   ?? throw new InvalidOperationException($"Failed to create casting value provider for target type {targetType.Name}");
        }

        private sealed class CastingNamedValueProvider<TOut, TIn> : INamedValueProvider<TOut>
        {
            private readonly INamedValueProvider<TIn> source;

            public CastingNamedValueProvider(INamedValueProvider<TIn> source)
            {
                this.source = source;
            }

            public TOut Value => (TOut)(object)source.Value!;
            public string Name => source.Name;
        }
    }
}
