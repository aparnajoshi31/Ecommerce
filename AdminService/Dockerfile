# Use the official image as a parent image.
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

# Use the .NET SDK for building our application
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AdminService.csproj", "./"]
RUN dotnet restore "./AdminService.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "AdminService.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "AdminService.csproj" -c Release -o /app/publish

# Final stage / image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AdminService.dll"]