#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["CAFFShop.Api/CAFFShop.Api.csproj", "CAFFShop.Api/"]
COPY ["CAFFShop.Application/CAFFShop.Application.csproj", "CAFFShop.Application/"]
COPY ["CAFFShop.Dal/CAFFShop.Dal.csproj", "CAFFShop.Dal/"]
RUN dotnet restore "CAFFShop.Api/CAFFShop.Api.csproj"
COPY . .
WORKDIR "/src/CAFFShop.Api"
RUN dotnet build "CAFFShop.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CAFFShop.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CAFFShop.Api.dll"]