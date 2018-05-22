#FROM microsoft/aspnetcore:1
#LABEL Name=userinterface Version=0.0.1
#ARG source=.
#WORKDIR /app
#EXPOSE 3000
#COPY $source .
#ENTRYPOINT dotnet userinterface.dll

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /code
COPY . .
RUN dotnet restore
RUN dotnet publish --output /output --configuration Release
FROM microsoft/aspnetcore:2.0
COPY --from=build /output /app
COPY --from=build /output/bin/Debug/netcoreapp2.0/userinterface.xml /app/userinterface.xml 
WORKDIR /app
EXPOSE 3000
ENTRYPOINT [ "dotnet", "userinterface.dll" ]