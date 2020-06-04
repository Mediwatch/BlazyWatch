#Docker file for Docker Image


FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.4-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1.300-buster AS build
WORKDIR /src
COPY ["Server/", "Server/"]
COPY ["Client/", "Client/"]
COPY ["Shared/", "Shared/"]
COPY ["SharedComponent/", "SharedComponent/"]
RUN dotnet restore "Server/Mediwatch.Server.csproj"
COPY . .
WORKDIR "/src/Server"
RUN dotnet build "Mediwatch.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Mediwatch.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mediwatch.Server.dll"]