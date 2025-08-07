# 📄 页面结构 & Handlebars 模板说明

## 📋 页面结构概览

| 页面 | 模板名称 | 功能说明 | 主要特性 |
|------|----------|----------|----------|
| **首页** | `home.hbs` | 显示提醒列表、推荐菜谱 | 统计卡片、快速操作、响应式布局 |
| **食材列表** | `foods/list.hbs` | 显示所有食材 | 搜索筛选、分页、状态标识 |
| **食材编辑** | `foods/edit.hbs` | 添加或修改食材表单 | 表单验证、分类选择、高级选项 |
| **菜谱列表** | `recipes/list.hbs` | 展示已有菜谱 | 可制作状态、筛选排序、标签系统 |
| **菜谱详情** | `recipes/detail.hbs` | 显示菜谱详情、可制作状态 | BOM清单、烹饪步骤、营养信息 |
| **新建菜谱** | `recipes/create.hbs` | 输入菜谱信息和BOM | 动态表单、食材选择、步骤编辑 |

---

## 🎨 模板设计特点

### 1. 现代化UI设计
- **渐变背景**：使用CSS渐变创建视觉层次
- **卡片布局**：信息分组清晰，易于扫描
- **状态标识**：颜色编码表示不同状态
- **响应式设计**：适配各种设备尺寸

### 2. 交互体验优化
- **实时搜索**：防抖处理，提升性能
- **模态框交互**：无刷新操作，流畅体验
- **表单验证**：客户端验证，即时反馈
- **加载状态**：提供视觉反馈

### 3. 数据可视化
- **进度条**：显示食材过期进度
- **统计卡片**：关键指标一目了然
- **状态徽章**：快速识别状态
- **图表展示**：食材可用性统计

---

## 📱 响应式布局

### 移动端适配
```css
@media (max-width: 768px) {
    .content-grid { grid-template-columns: 1fr; }
    .recipe-grid { grid-template-columns: 1fr; }
    .stats-grid { grid-template-columns: 1fr; }
}
```

### 平板端适配
```css
@media (min-width: 769px) and (max-width: 1024px) {
    .content-grid { grid-template-columns: 1fr 1fr; }
    .recipe-grid { grid-template-columns: repeat(2, 1fr); }
}
```

### 桌面端适配
```css
@media (min-width: 1025px) {
    .content-grid { grid-template-columns: 1fr 1fr; }
    .recipe-grid { grid-template-columns: repeat(auto-fill, minmax(300px, 1fr)); }
}
```

---

## 🔧 Handlebars 模板语法

### 基础语法
```handlebars
<!-- 变量输出 -->
{{variableName}}

<!-- 条件渲染 -->
{{#if condition}}
    内容
{{else}}
    其他内容
{{/if}}

<!-- 循环渲染 -->
{{#each items}}
    {{name}} - {{value}}
{{/each}}
```

### 助手函数
```handlebars
<!-- 日期格式化 -->
{{formatDate expiryDate}}

<!-- 状态判断 -->
{{#if isExpired}}
    <span class="status-badge expired">已过期</span>
{{else if isExpiring}}
    <span class="status-badge expiring">即将过期</span>
{{else}}
    <span class="status-badge normal">正常</span>
{{/if}}
```

### 嵌套数据结构
```handlebars
{{#each recipe.ingredients}}
    <div class="ingredient-item {{availabilityStatus}}">
        <div class="ingredient-name">{{name}}</div>
        <div class="ingredient-amount">{{quantity}} {{unit}}</div>
        {{#if isAvailable}}
            <span class="status-badge available">可用</span>
        {{else}}
            <span class="status-badge unavailable">不可用</span>
        {{/if}}
    </div>
{{/each}}
```

---

## 📊 数据绑定示例

### 首页数据
```javascript
{
    totalFoodItems: 25,
    expiringFoodItems: 3,
    expiredFoodItems: 1,
    totalRecipes: 12,
    reminders: [
        {
            id: "1",
            name: "苹果",
            quantity: 5,
            unit: "个",
            expiryDate: "2024-02-01",
            isExpired: false,
            isExpiring: true
        }
    ],
    recommendedRecipes: [
        {
            id: "1",
            name: "红烧肉",
            description: "经典家常菜",
            cookingTime: 45,
            difficulty: "中等",
            imageUrl: "/images/recipe1.jpg"
        }
    ]
}
```

### 食材列表数据
```javascript
{
    foodItems: [
        {
            id: "1",
            name: "苹果",
            quantity: 5,
            unit: "个",
            addedDate: "2024-01-25",
            expiryDate: "2024-02-01",
            status: "normal",
            isExpired: false,
            isExpiring: true,
            notes: "红富士苹果",
            expiryProgress: 70,
            daysUntilExpiry: 3
        }
    ],
    pagination: {
        currentPage: 1,
        totalPages: 5,
        hasPrevious: false,
        hasNext: true
    }
}
```

### 菜谱详情数据
```javascript
{
    recipe: {
        id: "1",
        name: "红烧肉",
        description: "经典家常菜，肥而不腻",
        cookingTime: 45,
        difficulty: "medium",
        difficultyText: "中等",
        servings: 4,
        ingredientCount: 8,
        isCookable: true,
        ingredients: [
            {
                id: "1",
                name: "五花肉",
                quantity: 500,
                unit: "克",
                isAvailable: true,
                isPartiallyAvailable: false,
                missingQuantity: 0
            }
        ],
        instructions: [
            "五花肉切成大块，冷水下锅焯水",
            "锅中放油，放入冰糖炒至焦糖色"
        ],
        nutritionInfo: {
            calories: 350,
            protein: 25,
            fat: 28,
            carbohydrates: 5
        }
    }
}
```

---

## 🎯 功能特性

### 1. 智能提醒系统
- **过期提醒**：自动计算并显示即将过期的食材
- **状态标识**：颜色编码区分正常、即将过期、已过期
- **进度显示**：可视化显示食材保质期进度

### 2. 菜谱推荐引擎
- **可制作状态**：基于现有食材判断菜谱可制作性
- **食材匹配**：显示缺少的食材和可用百分比
- **智能排序**：优先推荐使用即将过期食材的菜谱

### 3. 搜索和筛选
- **实时搜索**：支持食材名称、菜谱名称搜索
- **多维度筛选**：按状态、分类、难度等筛选
- **排序功能**：支持多种排序方式

### 4. 表单验证
- **客户端验证**：实时验证表单输入
- **错误提示**：友好的错误信息显示
- **必填字段**：清晰的必填字段标识

---

## 🚀 性能优化

### 1. 前端优化
- **防抖搜索**：减少不必要的API请求
- **懒加载**：图片和内容按需加载
- **缓存策略**：合理使用浏览器缓存

### 2. 模板优化
- **条件渲染**：避免不必要的DOM操作
- **数据预处理**：在服务端处理复杂计算
- **压缩输出**：减少传输数据量

### 3. 用户体验
- **加载状态**：提供视觉反馈
- **错误处理**：友好的错误提示
- **离线支持**：基础功能离线可用

---

## 🔄 扩展和维护

### 添加新页面
1. **创建模板文件**：在 `Views/Templates/` 下创建 `.hbs` 文件
2. **添加路由**：在 `Program.cs` 中添加路由配置
3. **创建样式**：在 `style.css` 中添加相应样式
4. **添加脚本**：创建对应的 JavaScript 文件

### 修改现有页面
1. **更新模板**：修改对应的 `.hbs` 文件
2. **调整样式**：更新 CSS 样式
3. **优化交互**：改进 JavaScript 逻辑
4. **测试验证**：确保功能正常

### 数据集成
1. **API对接**：连接后端API服务
2. **数据转换**：处理API返回的数据格式
3. **错误处理**：处理API调用异常
4. **缓存策略**：优化数据加载性能

---

## 📋 开发规范

### 命名规范
- **文件命名**：使用 kebab-case（如：food-list.hbs）
- **CSS类名**：使用 BEM 方法论
- **JavaScript变量**：使用 camelCase
- **模板变量**：使用 camelCase

### 代码组织
- **模板文件**：按功能模块组织
- **样式文件**：按组件类型组织
- **脚本文件**：按页面功能组织

### 注释规范
- **模板注释**：使用 `{{!-- 注释 --}}`
- **CSS注释**：使用 `/* 注释 */`
- **JS注释**：使用 `// 注释` 或 `/* 注释 */`

---

## 🎉 总结

这套页面结构和Handlebars模板系统为冰箱食物管理系统提供了：

✅ **完整的用户界面**：涵盖所有核心功能页面  
✅ **现代化设计**：美观、易用的界面设计  
✅ **响应式布局**：适配各种设备尺寸  
✅ **交互体验**：流畅的用户交互体验  
✅ **可扩展性**：易于维护和扩展的架构  
✅ **性能优化**：高效的前端性能表现  

通过这些模板，用户可以轻松管理食材库存、查看菜谱推荐、创建新菜谱，实现智能化的家庭食材管理！ 🏠🥬🍳 