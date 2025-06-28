# Flexible style isolation for Razor components

Consider this scenario: You've got a `CoolButton.razor` component that contains
a considerable amount of styles. Now you want to add another, very specific
button... that shares the common behavior with the `CoolButton.razor`! The
markup is similar, with some additions here and there. But thanks to the
style isolation, you'd have to duplicate the *whole* stylesheet to make the
styles apply to your component. Or, alternatively, define a common, fixed
CSS scope in your project file.

What if you could delegate defining a custom CSS scope to the build? Wouldn't
that be nice?

You're in luck, because that's *exactly* what this package does.

## Usage

Simply add a reference to `Swallow.Build.StyleIsolation` to your project and
define some `InheritStyles` items, each with an `Inherit` attribute pointing to
the component to inherit the styles from.

```xml
<ItemGroup>
  <PackageReference Include="Swallow.Build.StyleIsolation" PrivateAssets="all" ExcludeAssets="Runtime" />
  <InheritStyles Include="Components/SpecificButton.razor" Inherit="Components/CoolButton.razor" />
</ItemGroup>
```

This will cause `SpecificButton` to use the *exact* same CSS scope as
`CoolButton`, so that all styles defined for `CoolButton` will match the HTML
tags written by `SpecificButton`, next to all styles defined by `SpecificButton`
itself.

You'll need to *rebuild* (aka *clean* then *build*) once after defining new
`InheritStyles` items - MSBuild will think the styles are still up-to-date since
the file itself didn't change, but the added tag is wrong.

**Note**: Since the components share a common CSS scope, this also means that
`CoolButton` will *also* have access to the styles defined by `SpecificButton`.
