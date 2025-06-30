[![Swallow.Blazor.StaticAssetPaths](https://img.shields.io/nuget/v/Swallow.Blazor.StaticAssetPaths?style=for-the-badge&logo=nuget)](https://www.nuget.org/packages/Swallow.Blazor.StaticAssetPaths/)
&nbsp;
![Source Generator](https://img.shields.io/badge/c%23-source%20generator-blue?style=for-the-badge)
&nbsp;
![MIT license](https://img.shields.io/badge/license-mit-brightgreen?style=for-the-badge)

---

# Compile-time correct paths to static web assets

There's a mistake in this Blazor component. Can you spot it?

```csharp
<!DOCTYPE html>
<html lang="en">
    <head>
        <title>My cool page</title>
        <link rel="stylesheet" href="@Assets["global.css"]" />
        <link rel="stylesheet" href="@Assets["tehme.css"]" />
    </head>

    <body>
        <!-- Stuff. -->
    </body>
</html>
```

That's right, it's `tehme.css` instead of `theme.css`! Easy to stop, especially
when you've got many of these links across many components. But here's the
thing: These assets are known at compile time. Why is there no way to
have the compiler assist you when writing your code?

`Swallow.Blazor.StaticAssetPaths` is a source generator to give you *just that*:

```csharp
<!DOCTYPE html>
<html lang="en">
    <head>
        <title>My cool page</title>
        <link rel="stylesheet" href="@Assets[AssetPaths.Wwwroot.GlobalCss]" />
        <link rel="stylesheet" href="@Assets[AssetPaths.Wwwroot.ThemeCss]" />
    </head>

    <body>
        <!-- Stuff. -->
    </body>
</html>
```

Ah, way better. Compile-time constants. What does the generated file look like?

```csharp
namespace YourProject;

public static class AssetPaths
{
    public static class Wwwroot
    {
        /// <summary>
        /// Points to <c>wwwroot/global.css</c>
        /// </summary>
        public static readonly string GlobalCss = "global.css";

        /// <summary>
        /// Points to <c>wwwroot/theme.css</c>
        /// </summary>
        public static readonly string ThemeCss = "theme.css";
    }
}
```

You even get a small comment showing you the *exact* file path!

## Usage

Simply reference the package and you're good to go:

```xml
<PackageReference Include="Swallow.Blazor.StaticAssetPaths" />
```

By default, the generator will collect the following files:

* `wwwroot/**/*`
* `**/*.razor.js`
* `**/*.cshtml.js`

If you want to include some files, simply add these items to `AdditionalFiles`:

```xml
<AdditionalFiles Include="Resources/translations.js" SourceItemGroup="Content" />
```

If you want to *exclude* items, you can... well... exclude them... or you can
override the standard properties `DefaultItemExcludes` and
`DefaultExcludesInProjectFolder`.
