# Release Notes

## 5.0.0

- Update `Microsoft.Extensions.DependencyInjection.Abstractions` to 9.0.0
- Update `SimpleInjector` to 5.5.0

---

## 4.0.0

- Upgrade to .NET 8 and update all packages.

---

## 3.0.0

- Update all packages and dependencies to .NET 7
- Use the new logo!

---

## 2.1.0

- The implementation for a ServiceCollection is now published as separate NuGet package
- The implementation for SimpleInjector is now published as separate NuGet package
- Both implementations allow the default lifestyle to be configured

---

## 2.0.0

A tiny change in the code, a greater change in the package.

- Chain members can now be added with their own Lifestyle. This is typically provided by your DI-container (like `Lifestyle.Scoped`)
  - `AbstractChainConfigurator` has a third type parameter now - the type of the lifestyle
  - Deriving classes need to call the base constructor using the a default lifestyle to choose when none is given
  - The signature of `AbstractChainConfigurator.Register` has changed to include the lifestyle as well

The rest of the changes are in regards to the NuGet package; stuff like embedded debug symbols, SourceLink and all that.

--

## 1.1.0

- No change whatsoever, just a bump to .NET 5

---

## 1.0.0

- First release, wohoo!
