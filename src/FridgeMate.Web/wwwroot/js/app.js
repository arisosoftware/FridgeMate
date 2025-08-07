// 全局应用配置
const APP_CONFIG = {
    API_BASE_URL: '/api',
    DEFAULT_PAGE_SIZE: 10,
    DATE_FORMAT: 'YYYY-MM-DD',
    DATETIME_FORMAT: 'YYYY-MM-DD HH:mm:ss'
};

// 通用工具函数
const Utils = {
    // 格式化日期
    formatDate: function(date) {
        if (!date) return '';
        const d = new Date(date);
        return d.toLocaleDateString('zh-CN');
    },

    // 格式化日期时间
    formatDateTime: function(date) {
        if (!date) return '';
        const d = new Date(date);
        return d.toLocaleString('zh-CN');
    },

    // 计算天数差
    daysBetween: function(date1, date2) {
        const oneDay = 24 * 60 * 60 * 1000;
        const diffTime = new Date(date2) - new Date(date1);
        return Math.round(diffTime / oneDay);
    },

    // 显示通知
    showNotification: function(message, type = 'info') {
        const notification = document.createElement('div');
        notification.className = `notification notification-${type}`;
        notification.textContent = message;
        
        document.body.appendChild(notification);
        
        setTimeout(() => {
            notification.classList.add('show');
        }, 100);
        
        setTimeout(() => {
            notification.classList.remove('show');
            setTimeout(() => {
                document.body.removeChild(notification);
            }, 300);
        }, 3000);
    },

    // 确认对话框
    confirm: function(message, callback) {
        if (confirm(message)) {
            callback();
        }
    },

    // 获取URL参数
    getUrlParameter: function(name) {
        const urlParams = new URLSearchParams(window.location.search);
        return urlParams.get(name);
    },

    // 设置URL参数
    setUrlParameter: function(name, value) {
        const url = new URL(window.location);
        url.searchParams.set(name, value);
        window.history.pushState({}, '', url);
    },

    // 防抖函数
    debounce: function(func, wait) {
        let timeout;
        return function executedFunction(...args) {
            const later = () => {
                clearTimeout(timeout);
                func(...args);
            };
            clearTimeout(timeout);
            timeout = setTimeout(later, wait);
        };
    }
};

// API请求工具
const API = {
    // 基础请求方法
    request: async function(url, options = {}) {
        const defaultOptions = {
            headers: {
                'Content-Type': 'application/json',
            },
        };

        const finalOptions = { ...defaultOptions, ...options };

        try {
            const response = await fetch(`${APP_CONFIG.API_BASE_URL}${url}`, finalOptions);
            
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            
            return await response.json();
        } catch (error) {
            console.error('API request failed:', error);
            Utils.showNotification('请求失败，请稍后重试', 'error');
            throw error;
        }
    },

    // GET请求
    get: function(url, params = {}) {
        const queryString = new URLSearchParams(params).toString();
        const fullUrl = queryString ? `${url}?${queryString}` : url;
        return this.request(fullUrl);
    },

    // POST请求
    post: function(url, data = {}) {
        return this.request(url, {
            method: 'POST',
            body: JSON.stringify(data),
        });
    },

    // PUT请求
    put: function(url, data = {}) {
        return this.request(url, {
            method: 'PUT',
            body: JSON.stringify(data),
        });
    },

    // DELETE请求
    delete: function(url) {
        return this.request(url, {
            method: 'DELETE',
        });
    }
};

// 模态框管理
const Modal = {
    // 显示模态框
    show: function(modalId) {
        const modal = document.getElementById(modalId);
        if (modal) {
            modal.classList.add('show');
            document.body.style.overflow = 'hidden';
        }
    },

    // 隐藏模态框
    hide: function(modalId) {
        const modal = document.getElementById(modalId);
        if (modal) {
            modal.classList.remove('show');
            document.body.style.overflow = '';
        }
    },

    // 隐藏所有模态框
    hideAll: function() {
        const modals = document.querySelectorAll('.modal');
        modals.forEach(modal => {
            modal.classList.remove('show');
        });
        document.body.style.overflow = '';
    }
};

// 表单验证
const Validation = {
    // 验证必填字段
    required: function(value) {
        return value && value.trim().length > 0;
    },

    // 验证邮箱
    email: function(value) {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return emailRegex.test(value);
    },

    // 验证数字
    number: function(value) {
        return !isNaN(value) && value !== '';
    },

    // 验证最小长度
    minLength: function(value, min) {
        return value && value.length >= min;
    },

    // 验证最大长度
    maxLength: function(value, max) {
        return value && value.length <= max;
    },

    // 验证表单
    validateForm: function(form) {
        const inputs = form.querySelectorAll('[data-validate]');
        let isValid = true;

        inputs.forEach(input => {
            const rules = input.dataset.validate.split(',');
            let fieldValid = true;

            rules.forEach(rule => {
                const [ruleName, ruleValue] = rule.trim().split(':');
                
                switch (ruleName) {
                    case 'required':
                        if (!this.required(input.value)) {
                            fieldValid = false;
                            this.showError(input, '此字段为必填项');
                        }
                        break;
                    case 'email':
                        if (input.value && !this.email(input.value)) {
                            fieldValid = false;
                            this.showError(input, '请输入有效的邮箱地址');
                        }
                        break;
                    case 'number':
                        if (input.value && !this.number(input.value)) {
                            fieldValid = false;
                            this.showError(input, '请输入有效的数字');
                        }
                        break;
                    case 'minLength':
                        if (input.value && !this.minLength(input.value, parseInt(ruleValue))) {
                            fieldValid = false;
                            this.showError(input, `最少需要${ruleValue}个字符`);
                        }
                        break;
                    case 'maxLength':
                        if (input.value && !this.maxLength(input.value, parseInt(ruleValue))) {
                            fieldValid = false;
                            this.showError(input, `最多允许${ruleValue}个字符`);
                        }
                        break;
                }
            });

            if (fieldValid) {
                this.clearError(input);
            } else {
                isValid = false;
            }
        });

        return isValid;
    },

    // 显示错误信息
    showError: function(input, message) {
        this.clearError(input);
        
        const errorDiv = document.createElement('div');
        errorDiv.className = 'error-message';
        errorDiv.textContent = message;
        
        input.classList.add('error');
        input.parentNode.appendChild(errorDiv);
    },

    // 清除错误信息
    clearError: function(input) {
        input.classList.remove('error');
        const errorDiv = input.parentNode.querySelector('.error-message');
        if (errorDiv) {
            errorDiv.remove();
        }
    }
};

// 分页组件
const Pagination = {
    // 创建分页
    create: function(currentPage, totalPages, onPageChange) {
        const pagination = document.createElement('div');
        pagination.className = 'pagination';
        
        // 上一页
        if (currentPage > 1) {
            const prevBtn = document.createElement('button');
            prevBtn.className = 'btn btn-outline';
            prevBtn.innerHTML = '<i class="fas fa-chevron-left"></i> 上一页';
            prevBtn.onclick = () => onPageChange(currentPage - 1);
            pagination.appendChild(prevBtn);
        }
        
        // 页码信息
        const pageInfo = document.createElement('span');
        pageInfo.className = 'page-info';
        pageInfo.textContent = `第 ${currentPage} 页，共 ${totalPages} 页`;
        pagination.appendChild(pageInfo);
        
        // 下一页
        if (currentPage < totalPages) {
            const nextBtn = document.createElement('button');
            nextBtn.className = 'btn btn-outline';
            nextBtn.innerHTML = '下一页 <i class="fas fa-chevron-right"></i>';
            nextBtn.onclick = () => onPageChange(currentPage + 1);
            pagination.appendChild(nextBtn);
        }
        
        return pagination;
    }
};

// 搜索和筛选
const SearchFilter = {
    // 防抖搜索
    debouncedSearch: Utils.debounce(function(searchTerm, callback) {
        callback(searchTerm);
    }, 300),

    // 筛选数据
    filterData: function(data, filters) {
        return data.filter(item => {
            return Object.keys(filters).every(key => {
                const filterValue = filters[key];
                if (!filterValue) return true;
                
                const itemValue = item[key];
                if (typeof itemValue === 'string') {
                    return itemValue.toLowerCase().includes(filterValue.toLowerCase());
                }
                return itemValue === filterValue;
            });
        });
    },

    // 排序数据
    sortData: function(data, sortBy, sortOrder = 'asc') {
        return data.sort((a, b) => {
            let aValue = a[sortBy];
            let bValue = b[sortBy];
            
            if (typeof aValue === 'string') {
                aValue = aValue.toLowerCase();
                bValue = bValue.toLowerCase();
            }
            
            if (sortOrder === 'desc') {
                [aValue, bValue] = [bValue, aValue];
            }
            
            if (aValue < bValue) return -1;
            if (aValue > bValue) return 1;
            return 0;
        });
    }
};

// 本地存储工具
const Storage = {
    // 设置数据
    set: function(key, value) {
        try {
            localStorage.setItem(key, JSON.stringify(value));
        } catch (error) {
            console.error('Failed to save to localStorage:', error);
        }
    },

    // 获取数据
    get: function(key, defaultValue = null) {
        try {
            const item = localStorage.getItem(key);
            return item ? JSON.parse(item) : defaultValue;
        } catch (error) {
            console.error('Failed to read from localStorage:', error);
            return defaultValue;
        }
    },

    // 删除数据
    remove: function(key) {
        try {
            localStorage.removeItem(key);
        } catch (error) {
            console.error('Failed to remove from localStorage:', error);
        }
    },

    // 清空所有数据
    clear: function() {
        try {
            localStorage.clear();
        } catch (error) {
            console.error('Failed to clear localStorage:', error);
        }
    }
};

// 页面加载完成后初始化
document.addEventListener('DOMContentLoaded', function() {
    // 初始化模态框事件
    document.addEventListener('click', function(e) {
        if (e.target.classList.contains('modal')) {
            Modal.hideAll();
        }
    });

    // 初始化表单验证
    document.addEventListener('submit', function(e) {
        const form = e.target;
        if (form.dataset.validate !== undefined) {
            if (!Validation.validateForm(form)) {
                e.preventDefault();
                return false;
            }
        }
    });

    // 初始化搜索框
    const searchInputs = document.querySelectorAll('.search-box input');
    searchInputs.forEach(input => {
        input.addEventListener('input', function() {
            const searchTerm = this.value;
            const onSearch = this.dataset.onSearch;
            if (onSearch && typeof window[onSearch] === 'function') {
                SearchFilter.debouncedSearch(searchTerm, window[onSearch]);
            }
        });
    });

    // 初始化筛选器
    const filterSelects = document.querySelectorAll('.filter-options select');
    filterSelects.forEach(select => {
        select.addEventListener('change', function() {
            const filterValue = this.value;
            const onFilter = this.dataset.onFilter;
            if (onFilter && typeof window[onFilter] === 'function') {
                window[onFilter](filterValue);
            }
        });
    });
});

// 导出到全局作用域
window.Utils = Utils;
window.API = API;
window.Modal = Modal;
window.Validation = Validation;
window.Pagination = Pagination;
window.SearchFilter = SearchFilter;
window.Storage = Storage; 