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

@Component
export default class Dialog extends Vue {
    @Prop() model!: DialogViewModel;

    mounted() {
        // 这个地方是把model引用给了store，所以后续“变更”也能直接提交上去
        // 只要你不修改model的指针
        this.$store.commit("formData", this.model);
    }
}
</script>