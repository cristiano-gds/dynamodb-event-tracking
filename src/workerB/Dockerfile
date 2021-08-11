FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src

# copy csproj and restore as distinct layers
COPY *.csproj .
RUN dotnet restore

# copy and publish app and libraries
COPY . .
RUN dotnet build workerB.csproj -c Release -o /app/build
RUN dotnet publish -c release -o /app/publish

# final stage/image
FROM mcr.microsoft.com/dotnet/core/runtime:3.1
WORKDIR /app
COPY --from=build /app/publish .
RUN apt-get update
RUN apt-get install tzdata
RUN DEBIAN_FRONTEND=noninteractive apt-get install -y tzdata
ENTRYPOINT ["dotnet", "workerB.dll"]