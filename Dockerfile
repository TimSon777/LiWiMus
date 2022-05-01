FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY *.sln .
COPY . .
WORKDIR /app/src/LiWiMus.Web.MVC
RUN dotnet restore

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /app/src/LiWiMus.Web.MVC/out ./

ENTRYPOINT ["dotnet", "LiWiMus.Web.MVC.dll"]