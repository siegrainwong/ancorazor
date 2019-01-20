import Vue from 'vue';
import Vuex from 'vuex';
import DialogViewModel from "@/common/viewmodels/dialogViewModel.ts"

Vue.use(Vuex)

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
  }
})