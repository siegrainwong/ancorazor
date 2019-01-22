import Vue from 'vue';
import Router from 'vue-router';
import Home from './views/Home.vue';
import Form from './views/Form.vue';
import FormVuex from './views/FormVuex.vue';
import FormVuexDec from './views/FormVuex.Dec.vue';
import Login from './views/Login.vue';

import { getModule } from "vuex-module-decorators";
import store from "@/store";
import UserModule from "@/common/stores/userModule";

Vue.use(Router);

var router = new Router({
  mode: 'history',
  base: process.env.BASE_URL,
  routes: [
    {
      path: '/',
      name: 'home',
      component: Home,
      meta: {
        requireAuth: true // 添加该字段，表示进入这个路由是需要登录的
      }
    },
    {
      path: '/form',
      name: 'form',
      component: Form
    },
    {
      path: '/formvuex',
      name: 'formvuex',
      component: FormVuex
    },
    {
      path: '/formvuexdec',
      name: 'formvuexdec',
      component: FormVuexDec
    },
    {
      path: '/login',
      name: 'login',
      component: Login
    }
  ],
});

const module: UserModule = getModule(UserModule, store)
/**
 * 路由验证
 */
router.beforeEach((to, from, next) => {
  if (!to.meta.requireAuth) return next();

  // console.log("token:" + module.token)
  if (module.isTokenValid) return next();

  next({
    path: '/login',
    query: { redirect: to.fullPath }
  })
})

export default router;