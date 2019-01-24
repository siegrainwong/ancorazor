import { GetterTree, ActionContext, ActionTree, MutationTree, Commit } from 'vuex'
import axios from 'axios'
import { RootState } from 'store'

export const types = {}

export interface State { }

export const state = (): State => ({})

export const getters: GetterTree<State, RootState> = {}

export interface Actions<S, R> extends ActionTree<S, R> {
  nuxtServerInit(context: ActionContext<S, R>): void
}

export const mutations: MutationTree<State> = {}
