set -ex

cd $(dirname $0)/../src/

artifactsFolder="../artifacts"

if [ -d $artifactsFolder ]; then
  rm -R $artifactsFolder
fi

mkdir -p $artifactsFolder

dotnet restore ./Vulcan.DataAccess.sln


dotnet build ./Vulcan.DataAccess/Vulcan.DataAccess.csproj -c Release

versionNumber="2.1.1"

dotnet pack ./Vulcan.DataAccess/Vulcan.DataAccess.csproj -c Release -o ../$artifactsFolder --version-suffix=$versionNumber

pwd


dotnet nuget push ./$artifactsFolder/Vulcan.DataAccess.${versionNumber}.nupkg -k $NUGET_KEY -s https://www.nuget.org

