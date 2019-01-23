import Vue from 'vue';
import Vuex from 'vuex';
/**
 * New way using vuex-module-decorators
 */
import FormModule from "@/common/stores/formModule"
import { UserModule } from "@/common/stores/userModule"

// Vue.use(Vuex)

export default new Vuex.Store({
  modules: {
    // FormModule,
    // UserModule
  }
})