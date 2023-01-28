FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["On Air Endpoint.csproj", "./"]
RUN dotnet restore "On Air Endpoint.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "On Air Endpoint.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "On Air Endpoint.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "On Air Endpoint.dll"]
