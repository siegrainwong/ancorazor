<!-- 父组件 Form.vue -->

<template>
  <div class="parent">
    <!-- 注意这里，formData是自定义属性，用来向子组件传递数据，如果要想被子组件控制，必须加上 .sync-->
    <h1>问卷调查</h1>
    <child :formData.sync="form"></child>
    <div class>
      <p>姓名：{{form.name}}</p>
      <p>年龄：{{form.age}}</p>
      <p>地址：{{form.address}}</p>
    </div>
  </div>
</template>

<script>
import child from "../components/dialog.vue"; //导入子组件

export default {
  // components: {
  //   child
  // },
  data: function() {
    //定义返回data
    return {
      form: {
        name: "",
        namePla: "姓名不能为空",
        address: "",
        age: ""
      }
    };
  }
};
</script>