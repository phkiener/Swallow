using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;

namespace Swallow.Blazor.Reactive.Routing;

internal abstract class AttributeBasedDataSource<T> : EndpointDataSource where T : Attribute
{
    private readonly Lock lockObject = new();
    private readonly List<Assembly> assemblies = [];
    private readonly List<Action<EndpointBuilder>> conventions = [];
    private readonly List<Action<EndpointBuilder>> finallyConventions = [];

    private List<Endpoint>? endpoints;
    private CancellationTokenSource changeTokenSource;
    private IChangeToken changeToken;

    protected AttributeBasedDataSource()
    {
        ConventionBuilder = new EndpointConventionBuilder(this);
        GenerateChangeToken();
    }

    public IEndpointConventionBuilder ConventionBuilder { get; }

    public override IChangeToken GetChangeToken() => changeToken;

    public override IReadOnlyList<Endpoint> Endpoints
    {
        get
        {
            if (endpoints is null)
            {
                BuildEndpoints();
                GenerateChangeToken();
            }

            return endpoints;
        }
    }

    public void Include(Assembly assembly)
    {
        lock (lockObject)
        {
            assemblies.Add(assembly);
        }

        BuildEndpoints();
        GenerateChangeToken();
    }

    protected abstract IReadOnlyList<EndpointBuilder> BuildEndpoints(Type targetType);

    [MemberNotNull(nameof(endpoints))]
    private void BuildEndpoints()
    {
        var foundEndpoints = new List<Endpoint>();
        lock (lockObject)
        {
            var types = assemblies.SelectMany(static a => a.GetExportedTypes())
                .Where(a => a.GetCustomAttributes<T>().Any())
                .ToList();

            foreach (var type in types)
            {
                foreach (var builder in BuildEndpoints(type))
                {
                    foreach (var convention in conventions)
                    {
                        convention(builder);
                    }

                    foreach (var finalConvention in finallyConventions)
                    {
                        finalConvention(builder);
                    }

                    foundEndpoints.Add(builder.Build());
                }
            }
        }

        endpoints = foundEndpoints;
    }

    [MemberNotNull(nameof(changeTokenSource))]
    [MemberNotNull(nameof(changeToken))]
    private void GenerateChangeToken()
    {
        var previousChangeTokenSource = changeTokenSource;
        changeTokenSource = new CancellationTokenSource();
        changeToken = new CancellationChangeToken(changeTokenSource.Token);

        previousChangeTokenSource?.Cancel();
        previousChangeTokenSource?.Dispose();
    }

    private sealed class EndpointConventionBuilder(AttributeBasedDataSource<T> dataSource) : IEndpointConventionBuilder
    {
        public void Add(Action<EndpointBuilder> convention)
        {
            lock (dataSource.lockObject)
            {
                dataSource.conventions.Add(convention);
            }
        }

        public void Finally(Action<EndpointBuilder> finallyConvention)
        {
            lock (dataSource.lockObject)
            {
                dataSource.finallyConventions.Add(finallyConvention);
            }
        }
    }
}
