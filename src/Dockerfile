FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
# COPY Solar2InfluxDB.HuaweiSun2000/Solar2InfluxDB.HuaweiSun2000.csproj Solar2InfluxDB.HuaweiSun2000/
# COPY Solar2InfluxDB.InfluxDB/Solar2InfluxDB.InfluxDB.csproj Solar2InfluxDB.InfluxDB/
# COPY Solar2InfluxDB.Model/Solar2InfluxDB.Model.csproj Solar2InfluxDB.Model/
# COPY Solar2InfluxDB.Worker/Solar2InfluxDB.Worker.csproj Solar2InfluxDB.Worker/
# COPY Solar2InfluxDB.Startup/Solar2InfluxDB.Startup.csproj Solar2InfluxDB.Startup/

# RUN dotnet restore Solar2InfluxDB.Worker/Solar2InfluxDB.Worker.csproj
# RUN dotnet restore Solar2InfluxDB.Startup/Solar2InfluxDB.Startup.csproj
# RUN dotnet restore Solar2InfluxDB.Model/Solar2InfluxDB.Model.csproj
# RUN dotnet restore Solar2InfluxDB.HuaweiSun2000/Solar2InfluxDB.HuaweiSun2000.csproj
# RUN dotnet restore Solar2InfluxDB.InfluxDB/Solar2InfluxDB.InfluxDB.csproj

# copy and publish app and libraries
COPY . .
RUN dotnet publish Solar2InfluxDB.Startup/Solar2InfluxDB.Startup.csproj -c release -o /app

# final stage/image
FROM mcr.microsoft.com/dotnet/runtime:3.1
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["./Solar2InfluxDB.Startup"]