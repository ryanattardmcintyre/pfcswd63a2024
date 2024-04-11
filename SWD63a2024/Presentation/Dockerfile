# Use the official .NET 6 SDK image as a build stage.
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Copy the project file and restore dependencies.
COPY *.csproj ./
RUN dotnet restore

# Copy the remaining source code.
COPY . ./

# Build the application.
RUN dotnet publish -c Release -o out

# Use the official .NET 6 runtime image as the final base image.
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app

# Copy the built application from the build stage.
COPY --from=build /app/out ./

# Expose the port on which the application will run.
EXPOSE 80

# Run the application.
ENTRYPOINT ["dotnet", "Presentation.dll"]
