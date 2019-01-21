import Vue from 'vue';
import Vuex from 'vuex';
import DialogViewModel from "@/common/viewmodels/dialogViewModel"
/**
 * New way using vuex-module-decorators
 */
import FormModule from "@/common/stores/formModule"

Vue.use(Vuex)

/**
 * The traditional way to declare a vuex module
 */
export interface Module<S> {
  namespaced?: boolean;
  state?: S | (() => S);
  mutations?: {};
}
interface StateType {
  formDatas: DialogViewModel
}

export const OldModule2: Module<StateType> = {
  namespaced: true,
  state: {
    formDatas: new DialogViewModel()
  },
  mutations: {
    formData(state: any, data: DialogViewModel) {
      state.formDatas = data
    }
  }
};

export default new Vuex.Store({
  modules: {
    FormModule,
    OldModule2
  }
})