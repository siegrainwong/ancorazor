import Vue from 'vue';
import Vuex from 'vuex';
/**
 * New way using vuex-module-decorators
 */
import FormModule from "@/common/stores/formModule"
import UserModule from "@/common/stores/userModule"

Vue.use(Vuex)

/**
 * Knowledge: properly way to create vuex store
 * https://github.com/nuxt/nuxt.js/issues/757#issuecomment-303080933
 */
const store = () => {
  return new Vuex.Store({
    modules: {
      // FormModule,
      UserModule
    }
  })
}

export default store