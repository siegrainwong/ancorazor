<!-- 子组件 dialog.vue -->

<template>
    <div class="dialog">
        <label>
            姓名：
            <input :placeholder="model.nameValidation" type="text" v-model="model.name">
        </label>
        <label>
            年龄：
            <input type="text" v-model="model.age">
        </label>
        <label>
            地址：
            <input type="text" v-model="model.address">
        </label>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop, Watch, Emit } from "vue-property-decorator";
import DialogViewModel from "@/common/viewmodels/dialogViewModel.ts";
import { getModule } from "vuex-module-decorators";
import store from "@/store";
import FormModule from "@/common/stores/formModule";

@Component
export default class Dialog extends Vue {
    @Prop() model!: DialogViewModel;

    module: FormModule = getModule(FormModule, store);
    mounted() {
        this.module.setForm(this.model);
    }

    /**
     * 这里用来测试vuex是否真的存到了值，事实是是的。
     */
    @Watch("model", { immediate: true, deep: true })
    onModelChanged() {
        console.log(this.module.form);
    }
}
</script>