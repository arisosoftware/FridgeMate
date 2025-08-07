#!/bin/bash

echo "🧾 冰箱食物管理系统（FridgeMate）启动脚本"
echo "=========================================="

# 检查.NET SDK
if ! command -v dotnet &> /dev/null; then
    echo "❌ 错误：未找到.NET SDK，请先安装.NET 8.0"
    exit 1
fi

echo "✅ .NET SDK 已安装"

# 检查Docker
if command -v docker &> /dev/null; then
    echo "✅ Docker 已安装"
    
    echo "🐳 启动Docker服务..."
    cd docker
    docker-compose up -d
    
    echo "⏳ 等待服务启动..."
    sleep 10
    
    echo "🌐 服务已启动："
    echo "   - API: http://localhost:5000"
    echo "   - Swagger文档: http://localhost:5000/swagger"
    echo "   - PostgreSQL: localhost:5432"
    echo "   - Redis: localhost:6379"
    echo "   - pgAdmin: http://localhost:5050"
    
else
    echo "⚠️  Docker未安装，将使用本地开发模式"
    echo "📝 请确保已安装并启动PostgreSQL和Redis"
    echo "🚀 启动API服务..."
    cd src/FridgeMate.API
    dotnet run
fi 