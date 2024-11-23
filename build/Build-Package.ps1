param(
    # Name of the package to be packed
    [Parameter(Mandatory=$true, ValueFromPipeline=$true)]
    [string]$package,

    # Version to tag the packages with
    [Parameter(Mandatory=$true)]
    [string]$version
)

$testProjects = Get-ChildItem -Recurse "$package/test/**/*.csproj"
foreach ($project in $testProjects) {
    dotnet test $project --verbosity quiet
    if ($LASTEXITCODE -ne 0) {
        exit $LASTEXITCODE
    }
}

$packageProjects = Get-ChildItem -Recurse "$package/src/**/*.csproj"
foreach ($project in $packageProjects) {
    dotnet restore $project --verbosity quiet
    dotnet build --no-restore --configuration Release $project -p:PackageVersion=$version --verbosity quiet
    dotnet pack --no-restore --no-build --configuration Release --output "./publish/$package/" $project -p:PackageVersion=$version --verbosity quiet
}
