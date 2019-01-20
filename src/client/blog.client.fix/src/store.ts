import Vue from 'vue';
import Vuex from 'vuex';
import DialogViewModel from "@/common/viewmodels/dialogViewModel"

Vue.use(Vuex)

import { Module, VuexModule, Mutation } from 'vuex-module-decorators'

@Module({ name: "FormModule" })
export class FormModule extends VuexModule {
  form: DialogViewModel = new DialogViewModel()

  @Mutation
  setForm(val: DialogViewModel) {
    this.form = val
  }
}

interface StateType {
  formDatas: DialogViewModel
}

export default new Vuex.Store<StateType>({
  state: {
    formDatas: new DialogViewModel()
  },
  mutations: {
    formData(state, data: DialogViewModel) {
      state.formDatas = data
    }
  },
  modules: {
    FormModule
  }
})