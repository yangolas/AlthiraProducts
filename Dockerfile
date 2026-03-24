#1.- build (SDK)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

#2.- copy by section comparing with intenal docker hash
COPY AlthiraProducts.slnx ./
COPY src/AlthiraProducts.Adapters.*/*.csproj ./src/
COPY src/AlthiraProducts.BoundedContext.*/*.csproj ./src/
COPY src/AlthiraProducts.BuildingBlocks.*/*.csproj ./src/
COPY src/AlthiraProducts.Main/AlthiraProducts.Main.csproj ./src/AlthiraProducts.Main/
COPY src/AlthiraProducts.Main.Settings/AlthiraProducts.Main.Settings.csproj ./src/
COPY src/AlthiraProducts.Test.*/*.csproj ./src/

# 2.- Restore nuggets and libraries for main proyect, check cache if it is downladed in previous versions
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet restore "AlthiraProducts.slnx"

# 3.- Copy code source
COPY src/ ./src/

# 4. Publish Main Proyecto
WORKDIR /app

RUN pwd
RUN ls -R src/AlthiraProducts.Main
RUN mkdir -p /app/publish

# 4. Publish Main Proyecto
WORKDIR /app
RUN mkdir -p /app/publish

# Apuntamos al archivo EXACTO que queremos (el que no tiene .Settings)
RUN dotnet publish "src/AlthiraProducts.Main/AlthiraProducts.Main.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore \
    /p:UseAppHost=false \
    /p:CopyLocalLockFileAssemblies=true

# Verificamos que ahora sí se han creado las DLLs de los 16 proyectos
RUN ls -la /app/publish

# 5.- Execution
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "AlthiraProducts.Main.dll"]