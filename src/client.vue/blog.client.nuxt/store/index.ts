import Vuex from 'vuex';
import Vue from 'vue';
import * as root from './root'
/**
 * New way using vuex-module-decorators
 */
import FormModule from "../common/stores/formModule"
import UserModule from "../common/stores/userModule"
import { actions } from '../common/stores/getToken';

export type RootState = root.State

Vue.use(Vuex)

/**
 * Knowledge1: proper way to create vuex store in nuxtjs
 * https://github.com/nuxt/nuxt.js/issues/757#issuecomment-303080933
 * Knowledge2: 除了上面这条之外，nuxtjs中store文件名与页面名称是一一对应的
 */
const store = () => {
  return new Vuex.Store({
    modules: {
      UserModule
    },
    actions: actions
  })
}

export default store