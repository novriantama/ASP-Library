# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["ASP-Library.csproj", "./"]
RUN dotnet restore "ASP-Library.csproj"

# Copy source code and build
COPY . .
RUN dotnet publish "ASP-Library.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "ASP-Library.dll"]
