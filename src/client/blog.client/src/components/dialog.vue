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

@Component
export default class Dialog extends Vue {
  model!: DialogViewModel;
  /**
    Knowledge: Property
    属性起一个子组件暴露给父组件赋值的作用，子组件是不能直接对属性进行赋值的。
    所以这里会加上!:，告诉vue不要急，这是一会儿爸爸要传进来的值。
    也正因为子组件不能对属性赋值，所以我们要加一个watch监听这个属性，将属性值给model字段，子组件只能修改并绑定model字段。
    修改model自断后，通过.sync关键字内置的emit将model的值发射到父组件上。
   */
  @Prop() formData!: DialogViewModel;

  @Watch("formData", { immediate: true })
  onFormDataChanged(val: DialogViewModel) {
    this.model = val;
  }
}
export class DialogViewModel {
  name: string = "";
  nameValidation: string = "姓名不能为空";
  address: string = "";
  age: number = 18;
}
</script>