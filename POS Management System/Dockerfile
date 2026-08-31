FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["POS Management System.csproj", "./"]
RUN dotnet restore "POS Management System.csproj"
COPY . .
RUN dotnet publish "POS Management System.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# Default environment variables can be overridden at runtime
ENV ASPNETCORE_URLS="http://+:80"

ENTRYPOINT ["dotnet", "POS Management System.dll"]
