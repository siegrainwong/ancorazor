import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(Vuex)

export default new Vuex.Store({
  state: {
    formDatas: null
  },
  mutations: {
    getFormData(state, data) {
      state.formDatas = data
    }
  },
  actions: {

  }
})
