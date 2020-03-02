FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-env

# dotnet images don't come with npm installed anymore.
# to update the version change the version number and find the sha for the correct version at
# https://nodejs.org/download/release/<relase_number>/SHASUMS256.txt
# the variant currently used is the -linux-x64.tar.gz version

ENV NODE_VERSION 10.13.0
ENV NODE_DOWNLOAD_SHA b4b5d8f73148dcf277df413bb16827be476f4fa117cbbec2aaabc8cc0a8588e1

RUN curl -SL "https://nodejs.org/dist/v${NODE_VERSION}/node-v${NODE_VERSION}-linux-x64.tar.gz" --output nodejs.tar.gz \
    && echo "$NODE_DOWNLOAD_SHA nodejs.tar.gz" | sha256sum -c - \
    && tar -xzf "nodejs.tar.gz" -C /usr/local --strip-components=1 \
    && rm nodejs.tar.gz \
    && ln -s /usr/local/bin/node /usr/local/bin/nodejs

WORKDIR /app

# Copy current directory to a build directory
COPY . /build

WORKDIR /build/src/FOOBAR

# Build Front-End
RUN npm install && npm run build

# Build & package App (BFF)
RUN dotnet restore
RUN dotnet publish -c release -o /app --no-restore

WORKDIR /app

# Cleanup source ,nuget cache and dotnet sdk
RUN rm -rf /build && dotnet nuget locals all --clear && rm -rf /usr/share/dotnet/sdk

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2

ARG release
ENV RELEASE_VERSION=$release

ENV TZ Europe/Brussels

WORKDIR /app

COPY --from=build-env /app .
RUN mkdir -p log

CMD ["dotnet", "FOOBAR.dll"]
