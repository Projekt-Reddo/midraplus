FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS base
WORKDIR /app
EXPOSE 5277

ENV ASPNETCORE_URLS=http://+:5277

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

# Restore
FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
COPY *.sln .
COPY BoardService/*.csproj BoardService/
COPY BoardServiceTest/*.csproj BoardServiceTest/
RUN dotnet restore
COPY . .

# Test
WORKDIR "/src/BoardService"
RUN dotnet build "BoardService.csproj" -c Release -o /app/build
WORKDIR "/src/BoardServiceTest"
RUN dotnet test

# Publish
FROM build AS publish
WORKDIR /src/BoardService
RUN dotnet publish "BoardService.csproj" -c Release -o /app/publish /p:UseAppHost=true

# Run
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BoardService.dll"]
