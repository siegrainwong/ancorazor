# blog.client.fix

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

### Customize configuration
See [Configuration Reference](https://cli.vuejs.org/config/).

### 坑
这破项目刚create完run起来Chrome就报了manifest.json的错，还以为是正常现象。
然后开发到一半不知道怎么回事vue的lifecycle就挂了，直到重建项目重来一次才发现原来正常是不会报错的= =
