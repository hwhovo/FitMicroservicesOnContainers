FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Services/OnlineStore/OnlineStore.API/OnlineStore.API.csproj", "Services/OnlineStore/OnlineStore.API/"]
RUN dotnet restore "Services/OnlineStore/OnlineStore.API/OnlineStore.API.csproj"
COPY . .
WORKDIR "/src/Services/OnlineStore/OnlineStore.API"
RUN dotnet build "OnlineStore.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "OnlineStore.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "OnlineStore.API.dll"]