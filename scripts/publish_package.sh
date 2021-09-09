set -ex

cd $(dirname $0)/../

artifactsFolder="./artifacts"

if [ -d $artifactsFolder ]; then
  rm -R $artifactsFolder
fi

mkdir -p $artifactsFolder

dotnet restore ./src/Vulcan.DataAccess.sln


dotnet build ./src/Vulcan.DataAccess/Vulcan.DataAccess.csproj -c Release

versionNumber="2.1.3"

dotnet pack ./src/Vulcan.DataAccess/Vulcan.DataAccess.csproj -c Release -o $artifactsFolder --version-suffix=$versionNumber

pwd


dotnet nuget push $artifactsFolder/Vulcan.DataAccess.${versionNumber}.nupkg -k $NUGET_KEY -s https://www.nuget.org

