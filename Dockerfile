FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Install dependencies for certificate generation
RUN apt-get update && \
    apt-get install -y openssl && \
    rm -rf /var/lib/apt/lists/*

# Create certificate directory and generate cert
RUN mkdir -p /https && \
    dotnet dev-certs https -ep /https/aspnetapp.pfx -p YourSecurePassword

# Rest of the Dockerfile remains the same...
COPY browse_file_system.sln .
COPY API/API.csproj ./API/
COPY Core/Core.csproj ./Core/
COPY Infrastructure/Infrastructure.csproj ./Infrastructure/
RUN dotnet restore browse_file_system.sln
COPY . .
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .
RUN mkdir -p /https
COPY --from=build /https/aspnetapp.pfx /https/
ENTRYPOINT [ "dotnet", "API.dll" ]