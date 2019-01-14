# blog.client

## Project setup
```
npm install
```

### Compiles and hot-reloads for development
```
npm run serve
```

### Compiles and minifies for production
```
npm run build
```

### Run your tests
```
npm run test
```

### Lints and fixes files
```
npm run lint
```

### Run your unit tests
```
npm run test:unit
```

### Customize configuration
See [Configuration Reference](https://cli.vuejs.org/config/).

### Structure
```C#
├── public                      　　  　    // 项目公共文件夹
│   └── favicon.ico             　　 　     // 项目配置文件
│   └── index.html              　　  　    // 项目入口文件
├── src                        　　　　　    // 我们的项目的源码编写文件
│   ├── assets                  　　         // 基础样式存放目录
│   │   └── logo.png        　　　　　　　    // 基础图片文件
│   ├── components                          // 组件存放目录
│   │   └── HelloWorld.vue       　　　　    // helloworld组件
│   ├── views                               // view存放目录
│   │   ├── About.vue       　　　　　　　    //about 页面
│   │   └── Home.vue       　　　              //Home 页面
│   └── App.vue            　　　　　　　　   // App入口文件
│   └── main.js             　　　　　　　  　 // 主配置文件
│   └── router.js             　　　　　　 　　 // 路由配置文件
│   └── store.js             　　　　　　　 　 // Vuex store配置文件
├── tests                      　　  　      // 测试文件夹
│   ├── unit                  　　            // 单元测试
│   │   ├── .eslintrc       　　　　　　 　    // 基础图片文件
│   │   └── HelloWorld.spec.js        　　　
└── babel.config.js                           // babel 配置文件
└── package.json                             // 项目依赖包配置文件 
└── package-lock.json                        // npm5 新增文件，优化性能
└── postcss.config.js                        //  
└── README.md                                // 说明文档 
```