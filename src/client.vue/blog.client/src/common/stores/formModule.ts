import { Module, VuexModule, Mutation } from 'vuex-module-decorators'
import DialogViewModel from "@/common/viewmodels/dialogViewModel"

@Module({ name: "FormModule" })
export default class FormModule extends VuexModule {
    form: DialogViewModel = new DialogViewModel()

    @Mutation
    setForm(val: DialogViewModel) {
        this.form = val
    }
}