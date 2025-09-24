# https://mcr.microsoft.com/en-us/artifact/mar/dotnet/sdk/tags
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ./ ./subscriber
RUN dotnet restore "subscriber/subscriber.csproj"
WORKDIR /src/subscriber
RUN dotnet build "subscriber.csproj" --no-restore -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "subscriber.csproj" --no-restore -c Release -o /app/publish /p:UseAppHost=false

#COPY subscriber.csproj subscriber/
#RUN dotnet restore subscriber/subscriber.csproj
#COPY . subscriber/
#RUN dotnet publish subscriber/subscriber.csproj -c Release -o /app/publish --no-restore

# https://mcr.microsoft.com/en-us/artifact/mar/dotnet/aspnet/tags
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
#RUN apt-get update && apt-get install -y curl ca-certificates && rm -rf /var/lib/apt/lists/* \
# && curl -fsSL https://raw.githubusercontent.com/dapr/cli/master/install/install.sh | /bin/bash \
# && mv /usr/local/bin/daprd /usr/local/bin/daprd-bin || true
COPY --from=publish /app/publish .
#COPY components /components
#EXPOSE 8080
#COPY <<'SH' /entry.sh
##!/usr/bin/env sh
#set -e
#daprd --app-id subscriber --app-port 8080 --resources-path /components --log-level info &
#exec dotnet subscriber.dll
#SH
#RUN chmod +x /entry.sh
#ENTRYPOINT ["/entry.sh"]

ENTRYPOINT ["dotnet", "subscriber.dll"]