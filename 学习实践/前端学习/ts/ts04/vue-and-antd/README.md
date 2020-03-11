# 使用@vue/cli3从零创建一个antd前端项目

## 安装@vue/cli3
```
yarn install
```

### Compiles and hot-reloads for development
```
yarn serve
```

### Compiles and minifies for production
```
yarn build
```

### 在vue.config.js中设置antd
```
module.exports = {
    css: {
      loaderOptions: {
        less: {
          modifyVars: {
            'primary-color': '#1DA57A',
            'link-color': '#1DA57A',
            'border-radius-base': '2px',
          },
          javascriptEnabled: true
        }
      }
    }
  }
```

### Customize configuration
See [Configuration Reference](https://cli.vuejs.org/config/).
