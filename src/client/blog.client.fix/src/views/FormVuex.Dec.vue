
<!-- 父组件 parent.vue -->

<template>
    <div class="parent">
        <h3>问卷调查2</h3>
        <!-- 注意： 这里已经没有 .sync 了，但初始值依然应该是我们的父组件来传给子组件的 -->
        <Dialog :model="fatherData"/>
        <div class>
            <br>
            <br>
            <p>数据：{{fatherData}}</p>
        </div>
    </div>
</template>


<script lang="ts">
import { Component, Vue, Prop } from "vue-property-decorator";
import Dialog from "@/components/dialog.vuex.vue"; //导入子组件
import DialogViewModel from "@/common/viewmodels/dialogViewModel.ts";

@Component({ components: { Dialog } })
export default class Form extends Vue {
    /**
     * Knowledge: computed
     * 这就是vue中的计算属性
     * 我们把这个获取到，传给子组件，子组件修改后同步到store中，然后我们这里也就能取到了。
     */
    get fatherData(): DialogViewModel {
        return this.$store.state.formDatas as DialogViewModel;
    }
}
</script>