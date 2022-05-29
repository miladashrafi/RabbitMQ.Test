#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Consumer3/Consumer3.csproj", "Consumer3/"]
RUN dotnet restore "Consumer3/Consumer3.csproj"
COPY . .
WORKDIR "/src/Consumer3"
RUN dotnet build "Consumer3.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Consumer3.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Consumer3.dll"]