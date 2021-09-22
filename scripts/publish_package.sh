set -ex

cd $(dirname $0)/../

artifactsFolder="./artifacts"

if [ -d $artifactsFolder ]; then
  rm -R $artifactsFolder
fi

mkdir -p $artifactsFolder

dotnet restore ./Vulcan.sln


dotnet build ./src/Vulcan.DapperExtensions/Vulcan.DapperExtensions.csproj -c Release

versionNumber="2.1.4"

dotnet pack ./src/Vulcan.DataAccess/Vulcan.DapperExtensions.csproj -c Release -o $artifactsFolder --version-suffix=$versionNumber



dotnet nuget push $artifactsFolder/Vulcan.DapperExtensions.${versionNumber}.nupkg -k $NUGET_KEY -s https://www.nuget.org

