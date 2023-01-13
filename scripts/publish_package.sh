set -ex

cd $(dirname $0)/../

artifactsFolder="./artifacts"

if [ -d $artifactsFolder ]; then
  rm -R $artifactsFolder
fi

mkdir -p $artifactsFolder

dotnet restore ./Vulcan.sln


dotnet build ./src/Vulcan.DapperExtensions/Vulcan.DapperExtensions.csproj -c Release

dotnet pack ./src/Vulcan.DapperExtensions/Vulcan.DapperExtensions.csproj -c Release -o $artifactsFolder 

dotnet nuget push $artifactsFolder/Vulcan.DapperExtensions.*.nupkg -k $NUGET_KEY -s https://www.nuget.org

