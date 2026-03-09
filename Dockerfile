# ETAPA 1: Compilación (SDK)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# 1. Copiamos la solución para restaurar paquetes
COPY ["AlthiraProducts.slnx", "./"]

# 2. Copiamos todo el contenido de la carpeta src
COPY src/ ./src/

# 3. Restauramos las dependencias de toda la solución
RUN dotnet restore "AlthiraProducts.slnx"

# 4. Publicamos el proyecto Main (el ejecutable)
WORKDIR "/src/src/AlthiraProducts.Main"
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# ETAPA 2: Ejecución (Runtime mucho más ligero)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Exponemos el puerto de la API
EXPOSE 80
EXPOSE 443

# Por defecto arranca el Main, luego con variables de entorno 
# decidiremos si es API, Worker o Consumer.
ENTRYPOINT ["dotnet", "AlthiraProducts.Main.dll"]