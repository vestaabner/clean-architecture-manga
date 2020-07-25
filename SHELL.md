```
dotnet ef migrations add first -s src/WebApi/WebApi.csproj -p src/Infrastructure/Infrastructure.csproj -c MangaContext

dotnet ef database update -s src/WebApi/WebApi.csproj -p src/Infrastructure/Infrastructure.csproj -c MangaContext
```

cli 默认env为 [Production](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/environments?view=aspnetcore-3.1). 可指定env：
```
sudo ASPNETCORE_ENVIRONMENT=Production dotnet ef database update
$env:ASPNETCORE_ENVIRONMENT="Production"; dotnet ef database update
```
