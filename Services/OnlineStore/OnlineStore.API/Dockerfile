FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
RUN mkdir -p /src
WORKDIR /src

COPY FitMicorservicesOnContainers.sln src/
COPY docker-compose.dcproj src/
COPY BuildingBlocks/EventBus/EventBus.Core/EventBus.Core.csproj src/BuildingBlocks/EventBus/EventBus.Core
COPY BuildingBlocks/EventBus/EventBus.ServiceBus/EventBus.ServiceBus.csproj src/BuildingBlocks/EventBus/EventBus.ServiceBus
COPY BuildingBlocks/EventBus/EventBus.RabbitMQ/EventBus.RabbitMQ.csproj src/BuildingBlocks/EventBus/EventBus.RabbitMQ

COPY Services/OnlineStore/OnlineStore.Infrastructure/OnlineStore.Infrastructure.csproj src/Services/OnlineStore/OnlineStore.Infrastructure
COPY Services/OnlineStore/OnlineStore.Core/OnlineStore.Core.csproj src/Services/OnlineStore/OnlineStore.Core
COPY Services/OnlineStore/OnlineStore.DAL/OnlineStore.DAL.csproj src/Services/OnlineStore/OnlineStore.DAL
COPY Services/OnlineStore/OnlineStore.BLL/OnlineStore.BLL.csproj src/Services/OnlineStore/OnlineStore.BLL
COPY Services/OnlineStore/OnlineStore.API/OnlineStore.API.csproj src/Services/OnlineStore/OnlineStore.API

#COPY Services/ChatRoom/ChatRoom.Infrastructure/ChatRoom.Infrastructure.csproj src/Services/ChatRoom/ChatRoom.Infrastructure
#COPY Services/ChatRoom/ChatRoom.Core/ChatRoom.Core.csproj src/Services/ChatRoom/ChatRoom.Core
#COPY Services/ChatRoom/ChatRoom.DAL/ChatRoom.DAL.csproj src/Services/ChatRoom/ChatRoom.DAL
#COPY Services/ChatRoom/ChatRoom.BLL/ChatRoom.BLL.csproj src/Services/ChatRoom/ChatRoom.BLL
#COPY Services/ChatRoom/ChatRoom/ChatRoom.API.csproj src/Services/ChatRoom/ChatRoom/ChatRoom.API


COPY . .
WORKDIR Services/OnlineStore/OnlineStore.API/
RUN dotnet build OnlineStore.API.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish OnlineStore.API.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "OnlineStore.API.dll"]