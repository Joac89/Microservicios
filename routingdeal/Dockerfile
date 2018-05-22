
#FROM microsoft/aspnetcore:1
#LABEL Name=routingdeal Version=0.0.1
#ARG source=.
#WORKDIR /app
#EXPOSE 3001
#COPY $source .
#ENTRYPOINT dotnet routingdeal.dll

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /code
COPY . .
RUN dotnet restore
RUN dotnet publish --output /output --configuration Release
FROM microsoft/aspnetcore:2.0
COPY --from=build /output /app
COPY --from=build /output/bin/Debug/netcoreapp2.0/routingdeal.xml /app/routingdeal.xml 
WORKDIR /app
EXPOSE 3001
ENTRYPOINT [ "dotnet", "routingdeal.dll" ]
