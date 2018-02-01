set -ex

cd $(dirname $0)/../src/

artifactsFolder="../artifacts"

if [ -d $artifactsFolder ]; then
  rm -R $artifactsFolder
fi

mkdir -p $artifactsFolder

dotnet restore ./Vulcan.DataAccess.sln


dotnet build ./Vulcan.DataAccess/Vulcan.DataAccess.csproj -c Release

revision="${TRAVIS_BUILD_NUMBER:=1}"
versionNumber="1.0.${revision}"

dotnet pack ./Vulcan.DataAccess/Vulcan.DataAccess.csproj -c Release -o ../$artifactsFolder --version-suffix=$versionNumber

pwd

if [ "$TRAVIS_BRANCH" == "master" ]; then
    dotnet nuget push ./$artifactsFolder/Vulcan.DataAccess.${versionNumber}.nupkg -k $NUGET_KEY -s https://www.nuget.org
fi
