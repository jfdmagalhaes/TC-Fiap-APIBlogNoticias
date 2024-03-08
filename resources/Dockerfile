# Use a imagem base do .NET Core
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 8999

# Use a imagem base do SDK do .NET Core para compilacao
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/WebApi/WebApi.csproj", "WebApi/"]
RUN dotnet restore "WebApi/WebApi.csproj"
COPY ["src/Infrastructure/Infrastructure.csproj", "Infrastructure/"]
RUN dotnet restore "Infrastructure/Infrastructure.csproj"
COPY ["src/Domain/Domain.csproj", "Domain/"]
RUN dotnet restore "Domain/Domain.csproj"

RUN dotnet tool install --global dotnet-ef --version 7.0.0
ENV PATH $PATH:/app

COPY . .
WORKDIR "/src/src/WebApi"
RUN dotnet build "WebApi.csproj" -c Release -o /app/build

FROM build AS publish
WORKDIR "/src/src/WebApi"
RUN dotnet publish "WebApi.csproj" -c Release -o /app/publish

# Etapa final - criacao da imagem da aplicacao
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApi.dll"]