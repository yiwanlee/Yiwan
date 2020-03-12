# @vue/cli开发入门

## 安装@vue/cli
> [查看官方文档](https://cli.vuejs.org/zh/guide/)
```
npm install -g @vue/cli
# OR
yarn global add @vue/cli
```
windows下yarn安装，还需要有2步设置

1. nodejs的 “ 具体目录\node_global\bin\ ” 目录设置到windows的path中
2. 打开 “ 具体目录\node_global\bin\ ” 目录中的vue.cmd（如果使用yarn或vue命令提示 目录、文件夹、命令错误，也是按照此方法修改）
```
# 修改
@"%~dp0\C:\Users\Yiwanlee\AppData\Local\Yarn\Data\global\node_modules\.bin\vue.cmd"
# 为
@"C:\Users\Yiwanlee\AppData\Local\Yarn\Data\global\node_modules\.bin\vue.cmd"
# 就是去掉C:前的“%~dp0\”
```


## 创建vue-cli项目

1. 打开终端
2. cd命令 切换到指定目录
3. 创建vue-cli项目
```
vue create demo01
显示：Please pick a preset:(Use arrow keys)
根据需要直接回车 默认安装 或
自定义安装
```
4.自定义安装建议安装项目
```
1. Babel
2. Router
3. Vuex
4. CSS Pre-processors
5. Linter / Formatter
```


VSCode 安装 Vetur插件 识别.vue文件
VSCode 安装 Ant Design Vue helper插件 
