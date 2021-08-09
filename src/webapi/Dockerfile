# get information about images in https://docs.microsoft.com/pt-br/aspnet/core/host-and-deploy/docker/building-net-docker-images?view=aspnetcore-3.1 
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src

# copy csproj and restore as distinct layers
COPY *.csproj .
RUN dotnet restore

# copy and publish app and libraries
COPY . .
RUN dotnet build webapi.csproj -c Release -o /app/build
RUN dotnet publish -c release -o /app/publish --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "webapi.dll"]