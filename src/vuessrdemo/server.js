const Vue = require('vue')//引入 vue
const server = require('express')()//引入 express 服务框架
const fs = require('fs')

//读取 html 模版
const renderer = require('vue-server-renderer').createRenderer({
  template: fs.readFileSync('./index.html', 'utf-8')//文件地址路径
})
// 此参数是vue 生成Dom之外位置的数据  如vue生成的dom一般位于body中的某个元素容器中，
//此数据可在header标签等位置渲染，是renderer.renderToString()的第二个参数，
//第一个参数是vue实例，第三个参数是一个回调函数。
const context = {
  title: '老张的哲学',
  meta: ` <meta name="viewport" content="width=device-width, initial-scale=1" />
                  <meta name="description" content="vue-ssr">
                  <meta name="generator" content="GitBook 3.2.3">
      `
}
//定义服务
server.get('*', (req, res) => {
  //创建vue实例   主要用于替换index.html中body注释地方的内容，
  //index.html中 <!--vue-ssr-outlet-->的地方 ，约定俗成
  const app = new Vue({
    data: {
      url: req.url,
      data: ['C#', 'SQL', '.NET', '.NET CORE', 'VUE'],
      title: '我的技能列表'
    },
    //template 中的文本最外层一定要有容器包裹， 和vue的组件中是一样的，
    //只能有一个父级元素，这里是div！
    template: `
            <div>
                <p>{{title}}shit</p>
                <p v-for='item in data'>{{item}}</p>
            </div>
        `
  })

  //将 Vue app实例渲染为字符串  (其他的API自己看用法是一样的)
  renderer.renderToString(app, context, (err, html) => {
    if (err) {
      res.status(500).end('err:' + err)
      return
    }
    //将模版发送给浏览器
    res.end(html)
    //每次请求 都在node 服务器中打印
    console.log('success')
  })
})
//服务端口开启并监听
server.listen(8060, () => {
  console.log('server success!')
})