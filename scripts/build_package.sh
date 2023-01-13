set -ex

cd $(dirname $0)/../

artifactsFolder="./artifacts"

if [ -d $artifactsFolder ]; then
  rm -R $artifactsFolder
fi

mkdir -p $artifactsFolder


dotnet build ./src/Vulcan.DapperExtensions/Vulcan.DapperExtensions.csproj -c Release

dotnet pack ./src/Vulcan.DapperExtensions/Vulcan.DapperExtensions.csproj -c Release -o $artifactsFolder 
