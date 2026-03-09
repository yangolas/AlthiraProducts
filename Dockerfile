# ETAPA 1: Compilación (SDK)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# 1. Copiamos TODO el contenido actual al contenedor
# Esto evita errores de "ruta no encontrada" si la estructura de carpetas es compleja
COPY . .

# 2. Restauramos las dependencias usando el nuevo formato .slnx
RUN dotnet restore "AlthiraProducts.slnx"

# 3. Publicamos el proyecto Main
# Entramos en la carpeta donde está el proyecto ejecutable
WORKDIR "/src/src/AlthiraProducts.Main"
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# ETAPA 2: Ejecución (Runtime mucho más ligero)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Exponemos los puertos
EXPOSE 80
EXPOSE 443

# Ejecución del microservicio
ENTRYPOINT ["dotnet", "AlthiraProducts.Main.dll"]