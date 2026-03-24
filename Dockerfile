#1.- build (SDK)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

#2.- copy by section comparing with intenal docker hash
COPY . .

# 2.- Restore nuggets and libraries for main proyect, check cache if it is downladed in previous versions
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet restore "src/AlthiraProducts.Main/AlthiraProducts.Main.csproj"

# 3. Publish Main Proyecto
WORKDIR /app/src/AlthiraProducts.Main
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false /p:ErrorOnDuplicatePublishOutputFiles=false

# 5.- Execution
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "AlthiraProducts.Main.dll"]