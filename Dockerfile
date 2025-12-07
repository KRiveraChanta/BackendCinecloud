# Usa la imagen base de .NET SDK 9 para compilar
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia el archivo del proyecto y restaura dependencias
COPY ["BackendCine.csproj", "./"]
RUN dotnet restore "BackendCine.csproj"

# Copia todo el código fuente
COPY . .

# Compila la aplicación en modo Release
RUN dotnet build "BackendCine.csproj" -c Release -o /app/build

# Publica la aplicación
FROM build AS publish
RUN dotnet publish "BackendCine.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Imagen final para ejecutar la aplicación
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Expone el puerto (Railway usa la variable de entorno PORT)
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# Copia los archivos publicados
COPY --from=publish /app/publish .

# Comando de inicio
ENTRYPOINT ["dotnet", "BackendCine.dll"]