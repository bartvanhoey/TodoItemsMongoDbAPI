  
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["TodoItemsMongoDbAPI.csproj", ""]
RUN dotnet restore "./TodoItemsMongoDbAPI.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "TodoItemsMongoDbAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TodoItemsMongoDbAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TodoItemsMongoDbAPI.dll"]