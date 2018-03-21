FROM microsoft/aspnetcore-build:2.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
# COPY src/StarterKit/*.csproj ./
# COPY nuget.config ./
# RUN dotnet restore --configfile ./nuget.config

# Copy everything else and build
COPY . /app
RUN dotnet publish -c Release -o out

FROM library/node:4  AS node-env
WORKDIR /app

# Install npm and run bower & gulp
COPY --from=build-env /app /app

WORKDIR /app/src/StarterKit

RUN npm install 
RUN npm run bower
RUN npm run gulp

# Build runtime image
FROM microsoft/aspnetcore:2.0
WORKDIR /app
COPY --from=build-env /app/src/StarterKit/out .
COPY --from=node-env /app/src/StarterKit/Mvc ./Mvc
COPY --from=node-env /app/src/StarterKit/wwwroot ./wwwroot

RUN mkdir -p /log
#ENV ASPNETCORE_URLS http://*:80
EXPOSE 80

CMD ["dotnet", "StarterKit.dll"]
