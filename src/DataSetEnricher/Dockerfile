FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY certs/ca.crt /usr/local/share/ca-certificates/ca.crt
COPY certs/api.gleif.org.crt /usr/local/share/ca-certificates/api.gleif.org.crt
RUN echo "api.gleif.org.crt" >> /etc/ca-certificates.conf

RUN apt-get update && apt-get install -y ca-certificates
RUN update-ca-certificates
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["DataSetEnricher.csproj", "./"]
RUN dotnet restore "DataSetEnricher.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "DataSetEnricher.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DataSetEnricher.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DataSetEnricher.dll"]
