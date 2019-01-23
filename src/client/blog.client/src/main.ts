import Vue from 'vue';
import App from './App.vue';
import router from './router';
import store from './store';

import 'element-ui/lib/theme-chalk/index.css'
import ElementUI from 'element-ui'
Vue.use(ElementUI)
Vue.config.productionTip = false;

export function createApp(): Vue {
  const app = new Vue({
    router,
    store,
    render: (h) => h(App),
  }).$mount('#app');
  return app;
}
