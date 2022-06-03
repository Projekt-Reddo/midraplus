FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS base
WORKDIR /app
EXPOSE 5244

ENV ASPNETCORE_URLS=http://+:5244

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

# Restore
FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
COPY *.sln .
COPY AdminService/*.csproj AdminService/
COPY AdminServiceTest/*.csproj AdminServiceTest/
RUN dotnet restore
COPY . .

# Test
FROM build as test
WORKDIR /src/AdminService
RUN dotnet build "AdminService.csproj" -c Release -o /app/build
WORKDIR /src/AdminServiceTest
RUN dotnet test

# Publish
FROM build AS publish
WORKDIR /src/AdminService
RUN dotnet publish "AdminService.csproj" -c Release -o /app/publish /p:UseAppHost=true

# Run
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AdminService.dll"]
