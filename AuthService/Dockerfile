# Use the official Microsoft .NET 6.0 SDK image as the base image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env

# Set the working directory in the image to /app
WORKDIR /app

# Copy the .csproj file and restore the dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the application files
COPY . ./

# Build the application
RUN dotnet publish -c Release -o out

# Use the Microsoft .NET 6.0 runtime image as the base image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0

# Set the working directory in the image to /app
WORKDIR /app

# Copy the build output from the build stage
COPY --from=build-env /app/out .

# Set the command to run when the container starts
ENTRYPOINT ["dotnet", "AuthService.dll"]