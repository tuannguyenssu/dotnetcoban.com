FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
WORKDIR /src
COPY ["AspNetCorePrometheusTest/AspNetCorePrometheusTest.csproj", "AspNetCorePrometheusTest/"]

COPY . .
WORKDIR "/src/AspNetCorePrometheusTest"

RUN dotnet publish "AspNetCorePrometheusTest.csproj" -r linux-musl-x64 -c Release -o /app/publish /p:PublishSingleFile=true /p:PublishTrimmed=true /p:PublishReadyToRun=true

FROM mcr.microsoft.com/dotnet/core/runtime-deps:3.1-alpine AS final
RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
WORKDIR /app
EXPOSE 5000
COPY --from=build /app/publish .
ENTRYPOINT ["./AspNetCorePrometheusTest"]