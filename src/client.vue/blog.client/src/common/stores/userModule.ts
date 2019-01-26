import { Module, VuexModule, Mutation } from 'vuex-module-decorators'

@Module({ name: "UserModule" })
export default class UserModule extends VuexModule {
    token?: string = window.localStorage.token

    @Mutation
    setToken(token: string) {
        this.token = token
        window.localStorage.setItem("token", token);
    }

    get authToken(): string {
        return `Bearer ${this.token}`
    }

    get isTokenValid(): boolean {
        return Boolean(this.token && this.token.length >= 128)
    }
}

class UserModel {
    token?: string
}