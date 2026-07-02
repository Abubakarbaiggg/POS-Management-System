FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["POS Management System/POS Management System.csproj", "POS Management System/"]
RUN dotnet restore "POS Management System/POS Management System.csproj"

# Copy the rest of the sources and publish
COPY . .
WORKDIR /src/"POS Management System"
RUN dotnet publish "POS Management System.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Listen on port 80
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "POS Management System.dll"]
