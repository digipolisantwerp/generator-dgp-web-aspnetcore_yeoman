FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env

# dotnet images don't come with npm installed anymore.
# to update the version change the version number and find the sha for the correct version at
# https://nodejs.org/download/release/<release_number>/SHASUMS256.txt
# the variant currently used is the -linux-x64.tar.gz version

ENV NODE_VERSION 12.22.12
ENV NODE_DOWNLOAD_SHA ff92a45c4d03e8e270bec1ab337b8fff6e9de293dabfe7e8936a41f2fb0b202e

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
RUN dotnet restore && dotnet publish -c release -o /app

WORKDIR /app

# Cleanup source ,nuget cache and dotnet sdk
RUN rm -rf /build && dotnet nuget locals all --clear && rm -rf /usr/share/dotnet/sdk

FROM mcr.microsoft.com/dotnet/aspnet:6.0 as runtime

# Inject version
ARG release
ENV RELEASE_VERSION=$release

WORKDIR /app

COPY --from=build-env /app .
RUN mkdir -p log

CMD ["dotnet", "FOOBAR.dll"]
