# 多阶段构建
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 复制项目文件
COPY ["src/FridgeMate.API/FridgeMate.API.csproj", "src/FridgeMate.API/"]
COPY ["src/FridgeMate.Core/FridgeMate.Core.csproj", "src/FridgeMate.Core/"]
COPY ["src/FridgeMate.Infrastructure/FridgeMate.Infrastructure.csproj", "src/FridgeMate.Infrastructure/"]
COPY ["src/FridgeMate.Domain/FridgeMate.Domain.csproj", "src/FridgeMate.Domain/"]
COPY ["src/FridgeMate.Shared/FridgeMate.Shared.csproj", "src/FridgeMate.Shared/"]

# 还原包
RUN dotnet restore "src/FridgeMate.API/FridgeMate.API.csproj"

# 复制源代码
COPY . .

# 构建应用
RUN dotnet build "src/FridgeMate.API/FridgeMate.API.csproj" -c Release -o /app/build

# 发布应用
FROM build AS publish
RUN dotnet publish "src/FridgeMate.API/FridgeMate.API.csproj" -c Release -o /app/publish

# 运行时镜像
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "FridgeMate.API.dll"] 