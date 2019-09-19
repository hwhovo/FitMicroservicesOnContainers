FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /src
COPY ["ChatRoom/ChatRoom.API.csproj", "ChatRoom/"]

RUN dotnet restore "ChatRoom/ChatRoom.API.csproj"
COPY . .
WORKDIR "/src/ChatRoom"
RUN dotnet build "ChatRoom.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ChatRoom.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ChatRoom.API.dll"]