
<!-- 父组件 parent.vue -->

<template>
  <div class="parent">
    <h3>问卷调查2</h3>
    <!-- 注意： 这里已经没有 .sync 了，但初始值依然应该是我们的父组件来传给子组件的 -->
    <Dialog :model="model"/>
    <div class>
      <br>
      <br>
      <p>数据2：{{model}}</p>
    </div>
  </div>
</template>


<script lang="ts">
import { Component, Vue, Prop } from "vue-property-decorator";
import Dialog from "@/components/dialog.vuex.dec.vue";
import DialogViewModel from "@/common/viewmodels/dialogViewModel";
import { getModule } from "vuex-module-decorators";
import store from "@/store";
import FormModule from "@/common/stores/formModule";

@Component({ components: { Dialog } })
export default class Form extends Vue {
  /**
   * Knowledge: vuex-module-decorators
   * 这里按照官方的startup跑不通，我不知道为什么
   * 我这里直接这样getModule就能取到值
   */
  module: FormModule = getModule(FormModule, store);
  get model(): DialogViewModel {
    return this.module.form;
  }
}
</script>