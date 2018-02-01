set -ex

cd $(dirname $0)/../src/


dotnet restore ./Vulcan.DataAccess.sln


dotnet build ./Vulcan.DataAccess/Vulcan.DataAccess.csproj -c Release

