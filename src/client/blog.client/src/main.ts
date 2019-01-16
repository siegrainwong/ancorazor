// 入口文件，初始化vue实例并使用需要的插件
import Vue from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'
import './registerServiceWorker'
import api from './common/network/api.request'

Vue.config.productionTip = false

/*
  Knowledge:
  实际上这就是一个单页面应用
  通过锚点在一个页面上路由各个不同的组件
*/
new Vue({
  router,
  store,
  render: h => h(App)
}).$mount('#app')
