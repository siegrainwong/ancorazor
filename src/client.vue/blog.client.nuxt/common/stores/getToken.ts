export const actions = {
  async nuxtServerInit({ commit }: any, { app }: any) {
    const cookies = app.$cookies
    await commit('setToken', cookies.get("token"))
  }
}