#!/bin/bash
package="$1"
version="$2"

if [ -z "$package" ]; then
  echo "Usage: $0 <package> <version>"
  exit 1;
fi

if [ -z "$version" ]; then
  echo "Usage: $0 <package> <version>"
  exit 1;
fi

find "./projects/$package/test" -name "*.csproj" -print0 |
  while IFS= read -r -d '' testProject; do
    dotnet test $testProject --verbosity quiet

    if [ $? -ne 0 ]; then
      exit 1;
    fi
  done

find "./projects/$package/src" -name "*.csproj" -print0 |
  while IFS= read -r -d '' packageProject; do
    dotnet restore $packageProject --verbosity quiet
    dotnet build --no-restore --configuration Release $packageProject -p:Version=$version --verbosity quiet
    dotnet pack --no-restore --no-build --configuration Release --output "./publish/$package/" $packageProject -p:Version=$version --verbosity quiet
  done
