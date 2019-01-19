<!-- 父组件 Form.vue -->

<template>
  <div class="parent">
    <h1>问卷调查</h1>
    <!--
      Knowledge: .sync + components
      Dialog：组件名，也是你在下面 import 出来的名字
      formData: 在组件内定义的属性名，添加 .sync 关键字后可以让子父组件双向绑定form字段

      子父组件双向绑定存在一个非常重要的"同步"顺序：
      1. 初始化父组件，初始化form字段
      2. 通过.sync关键字将父组件form字段值传递到子组件的formData属性上
      3. 子组件通过watch监听formData属性将新值覆盖到model字段上
      4. 子组件通过model字段进行绑定
      5. 子组件修改model字段后通过.sync里的emit将model发射到父组件的formData上，更新父组件的form字段
      https://cn.vuejs.org/v2/guide/components-custom-events.html#sync-%E4%BF%AE%E9%A5%B0%E7%AC%A6
    -->
    <Dialog :formData.sync="form"/>
    <div>
      <p>姓名：{{form.name}}</p>
      <p>年龄：{{form.age}}</p>
      <p>地址：{{form.address}}</p>

      <input :placeholder="form.nameValidation" type="text" v-model="form.name">
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import Dialog, { DialogViewModel } from "@/components/dialog.vue"; //导入子组件

@Component({ components: { Dialog } })
export default class Form extends Vue {
  form: DialogViewModel = new DialogViewModel();
}
</script>