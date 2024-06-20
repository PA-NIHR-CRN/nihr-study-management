FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NIHR.StudyManagement.OutboxProcessor/NIHR.StudyManagement.OutboxProcessor.csproj", "NIHR.StudyManagement.OutboxProcessor/"]
COPY ["NIHR.StudyManagement.Domain/NIHR.StudyManagement.Domain.csproj", "NIHR.StudyManagement.Domain/"]
COPY ["NIHR.StudyManagement.Infrastructure/NIHR.StudyManagement.Infrastructure.csproj", "NIHR.StudyManagement.Infrastructure/"]
RUN dotnet restore "./NIHR.StudyManagement.OutboxProcessor/NIHR.StudyManagement.OutboxProcessor.csproj"
COPY . .
WORKDIR "/src/NIHR.StudyManagement.OutboxProcessor"
RUN dotnet build "./NIHR.StudyManagement.OutboxProcessor.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./NIHR.StudyManagement.OutboxProcessor.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NIHR.StudyManagement.OutboxProcessor.dll"]