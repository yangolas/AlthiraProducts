# ETAPA 1: Compilación (SDK)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# 1. Copiamos TODO
COPY . .

# 2. TRUCO: En lugar de restaurar la solución .slnx (que tiene rutas rotas),
# restauramos directamente el proyecto principal. 
# Esto hará que dotnet busque las dependencias por nombre, no por ruta de disco U:
RUN dotnet restore "src/AlthiraProducts.Main/AlthiraProducts.Main.csproj"

# 3. Publicamos el proyecto
WORKDIR "/src/src/AlthiraProducts.Main"
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# ETAPA 2: Ejecución
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "AlthiraProducts.Main.dll"]