import Vuex from 'vuex'
import { Module, VuexModule, Mutation } from 'vuex-module-decorators'
import Cookie from 'cookie-universal-nuxt'


export interface Module<S> {
    namespaced?: boolean;
    state?: S | (() => S);
    mutations?: {};
    getters?: any;
}

export const UserModule: Module<UserModel> = {
    namespaced: true,
    state: {
        token: ""
    },
    // mutations: {
    // },
    // getters: {
    //     cookie(): any {
    //         console.log(this.context)
    //         if (!this.context) return null;
    //         return this.context.rootState.$cookie
    //     },
    //     getToken(): string {
    //         return this.cookie.get("token")
    //     }
    // }
};

// Declare empty store first
// @Module({ name: "UserModule", stateFactory: true, store: store, dynamic: true })
// @Module({ name: "UserModule", stateFactory: true })
// export default class UserModule extends VuexModule {
//     token?: any = !this.cookie ? null : this.cookie.get("token")

//     @Mutation
//     setToken(token: string) {
//         console.log(this.context.rootState.$cookie)
//         this.token = token
//         this.cookie.set("token", token)
//     }

//     get authToken(): string {
//         return `Bearer ${this.token}`
//     }

//     get isTokenValid(): boolean {
//         return Boolean(this.token && this.token.length >= 128)
//     }

//     get cookie(): any {
//         console.log(this.context)
//         if (!this.context) return null;
//         return this.context.rootState.$cookie
//     }
// }

interface UserModel {
    token?: string
}