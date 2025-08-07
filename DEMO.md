# 🧾 冰箱食物管理系统演示

## 🚀 快速开始

### 1. 使用Docker启动（推荐）

```bash
# 启动所有服务
./scripts/start.sh

# 或者手动启动
cd docker
docker-compose up -d
```

### 2. 手动启动

```bash
# 确保PostgreSQL和Redis已启动
# 启动API服务
cd src/FridgeMate.API
dotnet run
```

## 🌐 访问地址

启动后可以访问以下服务：

- **API服务**: http://localhost:5000
- **Swagger文档**: http://localhost:5000/swagger
- **PostgreSQL**: localhost:5432
- **Redis**: localhost:6379
- **pgAdmin**: http://localhost:5050

## 📋 API示例

### 1. 添加食材

```bash
curl -X POST "http://localhost:5000/api/fooditems" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "苹果",
    "quantity": 5,
    "unit": "个",
    "addedDate": "2024-01-25T10:00:00Z",
    "expiryDate": "2024-02-01T10:00:00Z",
    "notes": "红富士苹果"
  }'
```

### 2. 获取所有食材

```bash
curl -X GET "http://localhost:5000/api/fooditems"
```

### 3. 分页获取食材

```bash
curl -X GET "http://localhost:5000/api/fooditems/paged?pageNumber=1&pageSize=10"
```

### 4. 获取即将过期的食材

```bash
curl -X GET "http://localhost:5000/api/fooditems/expiring"
```

### 5. 更新食材

```bash
curl -X PUT "http://localhost:5000/api/fooditems/{id}" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "苹果",
    "quantity": 3,
    "unit": "个",
    "addedDate": "2024-01-25T10:00:00Z",
    "expiryDate": "2024-02-01T10:00:00Z",
    "notes": "红富士苹果，已食用2个"
  }'
```

### 6. 删除食材

```bash
curl -X DELETE "http://localhost:5000/api/fooditems/{id}"
```

## 🧪 测试

```bash
# 运行单元测试
dotnet test

# 运行特定测试项目
dotnet test tests/FridgeMate.UnitTests
```

## 📊 数据库结构

系统包含以下主要表：

- **food_items**: 食材表
- **recipes**: 菜谱表
- **recipe_ingredients**: 菜谱食材关系表
- **reminders**: 提醒表
- **users**: 用户表

## 🔧 配置

主要配置文件：

- `src/FridgeMate.API/appsettings.json`: API配置
- `src/FridgeMate.API/appsettings.Development.json`: 开发环境配置
- `docker/docker-compose.yml`: Docker服务配置

## 🐳 Docker服务

系统包含以下Docker服务：

- **api**: .NET Core API服务
- **postgres**: PostgreSQL数据库
- **redis**: Redis缓存
- **pgadmin**: PostgreSQL管理界面

## 📝 开发说明

### 项目结构

```
FridgeMate/
├── src/
│   ├── FridgeMate.API/          # Web API层
│   ├── FridgeMate.Core/          # 业务逻辑层
│   ├── FridgeMate.Infrastructure/ # 数据访问层
│   ├── FridgeMate.Domain/        # 领域模型层
│   └── FridgeMate.Shared/        # 共享组件
├── tests/
│   ├── FridgeMate.UnitTests/     # 单元测试
│   └── FridgeMate.IntegrationTests/ # 集成测试
└── docker/                       # Docker配置
```

### 技术栈

- **.NET 8.0**: 主要开发框架
- **PostgreSQL**: 主数据库
- **Redis**: 缓存数据库
- **Entity Framework Core**: ORM框架
- **AutoMapper**: 对象映射
- **Serilog**: 日志记录
- **Swagger**: API文档

### 核心功能

1. **食材管理**: 添加、编辑、删除、查询食材
2. **状态计算**: 自动计算食材过期状态
3. **缓存优化**: 使用Redis缓存频繁访问数据
4. **分页查询**: 支持分页和筛选
5. **过期提醒**: 获取即将过期的食材

## 🎯 下一步计划

- [ ] 实现菜谱管理功能
- [ ] 实现智能推荐算法
- [ ] 添加用户认证
- [ ] 实现移动端应用
- [ ] 添加OCR识别功能
- [ ] 实现消息推送

---

**冰箱食物管理系统（FridgeMate）** - 让家庭食材管理更智能、更高效！ 🥬🍳📅 