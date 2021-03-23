dotnet build ./zSpec/zSpec.csproj --configuration Release
dotnet nuget push ./zSpec/bin/Release/zSpec.0.1.6.nupkg --api-key <ApiKey> --source https://api.nuget.org/v3/index.json
