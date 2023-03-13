#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CSSHarris/CSSHarris.csproj", "CSSHarris/"]
RUN dotnet restore "CSSHarris/CSSHarris.csproj"
COPY . .
WORKDIR "/src/CSSHarris"
RUN dotnet build "CSSHarris.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CSSHarris.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CSSHarris.dll"]