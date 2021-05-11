#Docker file for Docker Image


FROM mcr.microsoft.com/dotnet/aspnet:5.0.1-buster-slim-amd64 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0.201-alpine3.13-amd64 AS build
WORKDIR /src
COPY ["Server/", "Server/"]
COPY ["Client/", "Client/"]
COPY ["Shared/", "Shared/"]
COPY ["SharedComponent/", "SharedComponent/"]
COPY ["BlazingEditor/", "BlazingEditor/"]
RUN dotnet restore "Server/Mediwatch.Server.csproj"
COPY . .
WORKDIR "/src/Server"
RUN dotnet build "Mediwatch.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Mediwatch.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ["Server/Ressources", "Ressources/"]
COPY ["Server/InvoiceGenerator", "InvoiceGenerator/"]
ENTRYPOINT ["dotnet", "Mediwatch.Server.dll"]