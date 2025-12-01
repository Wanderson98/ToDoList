# ESTÁGIO 1: BUILD (Compilação)
# Usamos a imagem completa do SDK para compilar o código
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# 1. Copiar os arquivos de projeto (camadas) para aproveitar o cache do Docker
# Se você mudar código, mas não mudar dependências, o Docker pula essa parte (rápido!)

COPY ["ToDo.WebApi/ToDo.WebApi.csproj", "ToDo.WebApi/"]
COPY ["ToDo.Services/ToDo.Services.csproj", "ToDo.Services/"]
COPY ["ToDo.Infrastructure/ToDo.Infrastructure.csproj", "ToDo.Infrastructure/"]
COPY ["ToDo.Domain/ToDo.Domain.csproj", "ToDo.Domain/"]

# 2. Baixar as dependências (Restore)
RUN dotnet restore "ToDo.WebApi/ToDo.WebApi.csproj"

COPY . .

# 4. Compilar e Publicar (Gerar a DLL otimizada para produção)
WORKDIR "/src/ToDo.WebApi"
RUN dotnet publish "ToDo.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false


# ESTÁGIO 2: RUNTIME (Execução)
# Usamos uma imagem leve (apenas ASP.NET Core) para rodar a aplicação final
# Isso deixa o container pequeno e seguro
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Variáveis de ambiente padrão (podem ser sobrescritas no docker-compose)
ENV ASPNETCORE_URLS=http://+:8080

# O comando que inicia a API
ENTRYPOINT ["dotnet", "ToDo.WebApi.dll"]