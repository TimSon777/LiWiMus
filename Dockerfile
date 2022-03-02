FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/LiWiMus.Web/LiWiMus.Web.csproj", "LiWiMus.Web/"]
COPY ["src/LiWiMus.Infrastructure/LiWiMus.Infrastructure.csproj", "LiWiMus.Infrastructure/"]
COPY ["src/LiWiMus.Core/LiWiMus.Core.csproj", "LiWiMus.Core/"]
COPY ["src/LiWiMus.SharedKernel/LiWiMus.SharedKernel.csproj", "LiWiMus.SharedKernel/"]
RUN dotnet restore "src/LiWiMus.Web/LiWiMus.Web.csproj"
COPY . .
WORKDIR "/src/LiWiMus.Web"
RUN dotnet build "LiWiMus.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LiWiMus.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LiWiMus.Web.dll"]
