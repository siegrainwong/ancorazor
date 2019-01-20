import Vue from 'vue';
import Router from 'vue-router';
import Home from './views/Home.vue';
import Form from './views/Form.vue';
import FormVuex from './views/FormVuex.vue';

Vue.use(Router);

export default new Router({
  mode: 'history',
  base: process.env.BASE_URL,
  routes: [
    {
      path: '/',
      name: 'home',
      component: Home,
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
  ],
});
