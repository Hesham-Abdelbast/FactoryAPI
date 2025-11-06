# Use .NET 9 SDK to build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy all project files separately so restore can cache dependencies efficiently
COPY ["FactoryAPI/FactoryAPI.csproj", "FactoryAPI/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["DAL/DAL.csproj", "DAL/"]
COPY ["AppModels/AppModels.csproj", "AppModels/"]

# Copy the solution file
COPY ["FactoryAPI.sln", "."]

# Restore dependencies for the whole solution
RUN dotnet restore "FactoryAPI.sln"

# Copy the remaining source code
COPY . .

# Set working directory to the main API project
WORKDIR /src/FactoryAPI

# Build the API in Release mode
RUN dotnet build "FactoryAPI.csproj" -c Release -o /app/build

# Publish the app
FROM build AS publish
RUN dotnet publish "FactoryAPI.csproj" -c Release -o /app/publish

# Use runtime-only image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "FactoryAPI.dll"]
