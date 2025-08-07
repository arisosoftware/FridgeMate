# 🎨 FridgeMate Web 前端项目

## 📁 项目结构

```
FridgeMate.Web/
├── Views/
│   └── Templates/           # Handlebars模板文件
│       ├── home.hbs         # 首页模板
│       ├── foods/
│       │   ├── list.hbs     # 食材列表模板
│       │   └── edit.hbs     # 食材编辑模板
│       └── recipes/
│           ├── list.hbs     # 菜谱列表模板
│           ├── detail.hbs   # 菜谱详情模板
│           └── create.hbs   # 新建菜谱模板
├── wwwroot/                 # 静态资源
│   ├── css/
│   │   └── style.css       # 主样式文件
│   ├── js/
│   │   ├── app.js          # 通用JavaScript
│   │   ├── home.js         # 首页脚本
│   │   ├── foods.js        # 食材管理脚本
│   │   └── recipes.js      # 菜谱管理脚本
│   └── images/             # 图片资源
└── FridgeMate.Web.csproj   # 项目文件
```

## 🎯 页面结构说明

### 1. 首页 (home.hbs)
**功能：** 显示提醒列表、推荐菜谱、统计信息

**主要组件：**
- 欢迎横幅
- 统计卡片（总食材、即将过期、已过期、菜谱数量）
- 今日提醒列表
- 推荐菜谱网格
- 快速操作按钮

**数据绑定：**
```handlebars
{{totalFoodItems}}          # 总食材数量
{{expiringFoodItems}}       # 即将过期食材数量
{{expiredFoodItems}}        # 已过期食材数量
{{totalRecipes}}           # 菜谱总数
{{reminders}}              # 提醒列表
{{recommendedRecipes}}     # 推荐菜谱列表
```

### 2. 食材列表 (foods/list.hbs)
**功能：** 显示所有食材，支持搜索、筛选、分页

**主要组件：**
- 搜索框
- 筛选选项（状态、排序）
- 食材卡片网格
- 分页控件
- 添加食材模态框

**数据绑定：**
```handlebars
{{foodItems}}              # 食材列表
{{pagination}}             # 分页信息
{{status}}                 # 食材状态
{{expiryProgress}}         # 过期进度
{{daysUntilExpiry}}       # 剩余天数
```

### 3. 食材编辑 (foods/edit.hbs)
**功能：** 添加或修改食材信息

**主要组件：**
- 基本信息表单
- 数量和单位选择
- 日期信息
- 存储位置选择
- 备注信息
- 高级选项

**数据绑定：**
```handlebars
{{foodItem}}               # 食材信息（编辑时）
{{category}}               # 分类选项
{{unit}}                   # 单位选项
{{storageLocation}}        # 存储位置选项
```

### 4. 菜谱列表 (recipes/list.hbs)
**功能：** 展示已有菜谱，支持筛选和搜索

**主要组件：**
- 搜索和筛选
- 菜谱卡片网格
- 可制作状态标识
- 分页控件
- 新建菜谱模态框

**数据绑定：**
```handlebars
{{recipes}}                # 菜谱列表
{{cookableStatus}}         # 可制作状态
{{ingredientCount}}        # 食材数量
{{missingIngredients}}     # 缺少的食材
{{availablePercentage}}    # 食材可用百分比
```

### 5. 菜谱详情 (recipes/detail.hbs)
**功能：** 显示菜谱详情、可制作状态、食材清单

**主要组件：**
- 菜谱头部信息
- 食材清单（BOM）
- 烹饪步骤
- 营养信息
- 相关菜谱
- 评论和评分

**数据绑定：**
```handlebars
{{recipe}}                 # 菜谱详细信息
{{ingredients}}            # 食材清单
{{instructions}}           # 烹饪步骤
{{nutritionInfo}}          # 营养信息
{{relatedRecipes}}         # 相关菜谱
{{reviews}}                # 评论列表
```

### 6. 新建菜谱 (recipes/create.hbs)
**功能：** 输入菜谱信息和BOM

**主要组件：**
- 基本信息表单
- 烹饪信息
- 食材清单（BOM）
- 烹饪步骤
- 营养信息
- 高级选项

**数据绑定：**
```handlebars
{{ingredients}}            # 食材列表
{{instructions}}           # 步骤列表
{{nutrition}}              # 营养信息
```

## 🎨 样式系统

### 设计原则
- **现代化UI**：使用渐变、阴影、圆角等现代设计元素
- **响应式设计**：支持桌面、平板、手机等不同设备
- **一致性**：统一的颜色、字体、间距规范
- **可访问性**：支持键盘导航、屏幕阅读器

### 颜色方案
```css
/* 主色调 */
--primary-color: #667eea;
--secondary-color: #764ba2;

/* 状态颜色 */
--success-color: #27ae60;
--warning-color: #f39c12;
--danger-color: #e74c3c;

/* 中性色 */
--text-primary: #2c3e50;
--text-secondary: #7f8c8d;
--background: #f8f9fa;
```

### 组件样式
- **卡片组件**：白色背景、圆角、阴影
- **按钮组件**：主色、轮廓、危险等变体
- **状态徽章**：不同颜色表示不同状态
- **模态框**：居中显示、背景遮罩

## 🔧 JavaScript功能

### 核心功能
- **API通信**：统一的API请求处理
- **表单验证**：客户端表单验证
- **模态框管理**：显示/隐藏模态框
- **搜索筛选**：实时搜索和筛选
- **分页处理**：动态分页加载

### 工具函数
```javascript
// 日期格式化
Utils.formatDate(date)

// API请求
API.get(url, params)
API.post(url, data)

// 模态框控制
Modal.show(modalId)
Modal.hide(modalId)

// 表单验证
Validation.validateForm(form)
```

## 📱 响应式设计

### 断点设置
```css
/* 移动设备 */
@media (max-width: 768px) {
    /* 单列布局 */
    /* 简化导航 */
    /* 调整间距 */
}

/* 平板设备 */
@media (min-width: 769px) and (max-width: 1024px) {
    /* 双列布局 */
    /* 中等间距 */
}

/* 桌面设备 */
@media (min-width: 1025px) {
    /* 多列布局 */
    /* 完整功能 */
}
```

## 🚀 使用说明

### 1. 启动项目
```bash
cd src/FridgeMate.Web
dotnet run
```

### 2. 访问页面
- **首页**：`http://localhost:5000/`
- **食材管理**：`http://localhost:5000/foods`
- **菜谱管理**：`http://localhost:5000/recipes`

### 3. 模板渲染
模板使用Handlebars.js进行渲染，支持：
- 条件渲染：`{{#if condition}}...{{/if}}`
- 循环渲染：`{{#each items}}...{{/each}}`
- 变量输出：`{{variableName}}`
- 助手函数：`{{formatDate date}}`

### 4. 数据绑定
所有模板都支持动态数据绑定，数据通过API获取并传递给模板进行渲染。

## 🔄 更新和维护

### 添加新页面
1. 在`Views/Templates/`下创建新的`.hbs`文件
2. 添加对应的CSS样式
3. 创建JavaScript文件处理交互逻辑
4. 更新路由配置

### 修改样式
- 主样式文件：`wwwroot/css/style.css`
- 组件样式：在对应组件中添加
- 响应式样式：使用媒体查询

### 添加功能
- 在`wwwroot/js/`下创建新的JavaScript文件
- 在HTML模板中引入脚本
- 实现相应的API调用和DOM操作

## 📋 开发规范

### 命名规范
- **文件命名**：使用kebab-case（如：food-list.js）
- **CSS类名**：使用BEM方法论
- **JavaScript变量**：使用camelCase
- **模板变量**：使用camelCase

### 代码组织
- **模板文件**：按功能模块组织
- **样式文件**：按组件类型组织
- **脚本文件**：按页面功能组织

### 性能优化
- **图片优化**：使用WebP格式，提供fallback
- **CSS优化**：压缩CSS，移除未使用样式
- **JavaScript优化**：代码分割，懒加载
- **缓存策略**：合理设置缓存头

---

**FridgeMate Web** - 为冰箱食物管理系统提供现代化的用户界面！ 🎨✨ 