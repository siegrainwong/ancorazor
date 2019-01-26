
import { Module, VuexModule, Mutation, Action } from 'vuex-module-decorators'

@Module({ name: "UserModule" })
export default class UserModule extends VuexModule {
    token?: any = ""

    @Mutation
    setToken(token: string) {
        this.token = token
        // this.cookie.set("token", token)
    }

    get authToken(): string {
        return `Bearer ${this.token}`
    }

    get isTokenValid(): boolean {
        return Boolean(this.token && this.token.length >= 128)
    }

    get cookie(): any {
        console.log(this.context)
        if (!this.context) return null;
        return this.context.rootState.$cookie
    }
}

interface UserModel {
    token?: string
}