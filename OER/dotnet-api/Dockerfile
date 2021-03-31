FROM microsoft/dotnet:2.2-aspnetcore-runtime
# Install libwkhtmltox dependencies
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        zlib1g \
        fontconfig \
        libfreetype6 \
        libx11-6 \
        libxext6 \
		libgdiplus \
        libxrender1 \
    && curl -o /usr/lib/libwkhtmltox.so \
        --location \
        https://github.com/rdvojmoc/DinkToPdf/raw/v1.0.8/v0.12.4/64%20bit/libwkhtmltox.so
WORKDIR /app
COPY /app /app
ENTRYPOINT ["dotnet","OERService.dll"]