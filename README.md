# 🧾 冰箱食物管理系统（FridgeMate）

[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-15+-green.svg)](https://www.postgresql.org/)
[![Redis](https://img.shields.io/badge/Redis-7.0+-red.svg)](https://redis.io/)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

## 📖 项目简介

冰箱食物管理系统（FridgeMate）是一个智能化的家庭食材管理平台，旨在帮助用户有效管理冰箱中的食材库存，追踪食材保质期，并根据即将过期的食材推荐可做的菜谱，减少浪费，提升家庭饮食效率。

### 🎯 核心功能

- 🥬 **食材管理**：入库、更新、出库、过期提醒
- 📅 **智能提醒**：提醒即将过期的食材
- 🍳 **菜谱推荐**：基于现有食材智能推荐可做的菜谱
- 📊 **库存可视化**：实时查看冰箱库存状态
- 🔄 **缓存优化**：使用 Redis 提高系统响应效率

### 🚀 技术栈

| 技术 | 版本 | 说明 |
|------|------|------|
| **.NET Core** | 8.0 | 跨平台开发框架 |
| **PostgreSQL** | 15+ | 主数据库 |
| **Redis** | 7.0+ | 缓存数据库 |
| **Entity Framework Core** | 8.0 | ORM框架 |
| **AutoMapper** | 12.0+ | 对象映射 |
| **FluentValidation** | 11.0+ | 数据验证 |
| **Serilog** | 3.0+ | 日志记录 |

## 📁 项目结构

```
FridgeMate/
├── 📁 docs/                          # 项目文档
│   ├── 📄 SRS_冰箱食物管理系统.md     # 软件需求规格说明书
│   └── 📄 项目结构示例.md             # 项目结构说明
├── 📁 src/                           # 源代码
│   ├── 📁 FridgeMate.API/           # Web API层
│   ├── 📁 FridgeMate.Core/          # 业务逻辑层
│   ├── 📁 FridgeMate.Infrastructure/ # 数据访问层
│   ├── 📁 FridgeMate.Domain/        # 领域模型层
│   └── 📁 FridgeMate.Shared/        # 共享组件
├── 📁 tests/                         # 测试项目
└── 📁 docker/                        # Docker配置
```

## 🚀 快速开始

### 环境要求

- .NET 8.0 SDK
- PostgreSQL 15+
- Redis 7.0+
- Docker & Docker Compose (可选)

### 使用 Docker 启动（推荐）

```bash
# 克隆项目
git clone https://github.com/your-username/fridgemate.git
cd fridgemate

# 启动所有服务
cd docker
docker-compose up -d

# 访问应用
# API文档：http://localhost:5001/swagger
# 应用：http://localhost:5001
```

### 手动启动

```bash
# 克隆项目
git clone https://github.com/your-username/fridgemate.git
cd fridgemate

# 安装依赖
dotnet restore

# 配置数据库连接
# 编辑 src/FridgeMate.API/appsettings.json

# 运行数据库迁移
dotnet ef database update --project src/FridgeMate.Infrastructure --startup-project src/FridgeMate.API

# 启动应用
dotnet run --project src/FridgeMate.API
```

## 📚 文档

- [📋 软件需求规格说明书](docs/SRS_冰箱食物管理系统.md) - 详细的功能需求和技术规格
- [📁 项目结构示例](docs/项目结构示例.md) - 代码组织方式和架构设计
- [🔧 API文档](http://localhost:5001/swagger) - 在线API文档（启动后访问）

## 🏗️ 系统架构

### 分层架构

```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   用户界面层    │    │   业务逻辑层    │    │   数据访问层    │
│  (Web/Mobile)   │◄──►│  (Service Layer) │◄──►│  (Repository)   │
└─────────────────┘    └─────────────────┘    └─────────────────┘
         │                       │                       │
         ▼                       ▼                       ▼
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   缓存层        │    │   定时任务层    │    │   数据库层      │
│  (Redis)        │    │  (Background)   │    │  (PostgreSQL)   │
└─────────────────┘    └─────────────────┘    └─────────────────┘
```

### 核心模块

1. **食材管理模块**
   - 添加、编辑、删除食材
   - 自动状态计算（正常、即将过期、已过期）
   - 多维度查询和筛选

2. **菜谱管理模块**
   - 菜谱录入和管理
   - 食材清单（BOM）关联
   - 菜谱难度和烹饪时间

3. **智能推荐模块**
   - 基于库存推荐菜谱
   - 优先推荐使用即将过期食材的菜谱
   - 采购建议生成

4. **提醒服务模块**
   - 定时扫描过期食材
   - 多渠道通知提醒
   - 今日待处理清单

## 🧪 测试

```bash
# 运行单元测试
dotnet test tests/FridgeMate.UnitTests

# 运行集成测试
dotnet test tests/FridgeMate.IntegrationTests

# 生成测试覆盖率报告
dotnet test --collect:"XPlat Code Coverage"
```

## 🐳 Docker 部署

### 开发环境

```bash
cd docker
docker-compose up -d
```

### 生产环境

```bash
cd docker
docker-compose -f docker-compose.prod.yml up -d
```

## 📊 数据库设计

### 核心表结构

| 表名 | 描述 | 主要字段 |
|------|------|----------|
| `food_items` | 食材表 | id, name, quantity, unit, expiry_date, status |
| `recipes` | 菜谱表 | id, name, description, steps, cooking_time |
| `recipe_ingredients` | 菜谱食材关系 | recipe_id, ingredient_id, quantity, unit |
| `reminders` | 提醒表 | id, food_item_id, type, message, is_read |

### 状态枚举

```csharp
public enum FoodStatus
{
    Normal,           // 正常（距离过期 > 48小时）
    NearlyExpired,    // 即将过期（距离过期 ≤ 48小时）
    Expired          // 已过期（已超过过期时间）
}
```

## 🔧 配置说明

### 数据库连接

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=fridgemate;Username=postgres;Password=password"
  }
}
```

### Redis 配置

```json
{
  "Redis": {
    "ConnectionString": "localhost:6379",
    "Database": 0,
    "KeyPrefix": "fridgemate:",
    "DefaultExpiration": "00:30:00"
  }
}
```

## 🚀 性能优化

### 缓存策略

- **推荐菜谱列表**：30分钟缓存
- **即将过期食材列表**：15分钟缓存
- **常用菜谱**：1小时缓存

### 数据库优化

- 关键字段建立索引
- 使用分页查询
- 软删除机制

## 🔒 安全特性

- 输入验证和SQL注入防护
- 敏感数据加密存储
- 请求日志记录
- 异常处理机制

## 🤝 贡献指南

1. Fork 项目
2. 创建功能分支 (`git checkout -b feature/AmazingFeature`)
3. 提交更改 (`git commit -m 'Add some AmazingFeature'`)
4. 推送到分支 (`git push origin feature/AmazingFeature`)
5. 打开 Pull Request

## 📝 开发规范

### 代码规范

- 遵循 C# 编码规范
- 使用 PascalCase 命名类和方法
- 使用 camelCase 命名变量
- 所有公共方法必须有 XML 文档注释

### 提交规范

```
feat: 添加新功能
fix: 修复bug
docs: 更新文档
style: 代码格式调整
refactor: 代码重构
test: 添加测试
chore: 构建过程或辅助工具的变动
```

## 📄 许可证

本项目采用 MIT 许可证 - 查看 [LICENSE](LICENSE) 文件了解详情。

## 📞 联系方式

- **项目负责人**：[姓名]
- **技术负责人**：[姓名]
- **邮箱**：[邮箱地址]
- **项目仓库**：[GitHub链接]

## 🙏 致谢

感谢所有为这个项目做出贡献的开发者和用户！

---

**冰箱食物管理系统（FridgeMate）** - 让家庭食材管理更智能、更高效！ 🥬🍳📅 