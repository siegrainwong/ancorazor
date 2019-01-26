<template>
  <el-row type="flex" justify="center">
    <el-form ref="loginForm" :model="user" :rules="rules" status-icon label-width="50px">
      <el-form-item label="账号" prop="name">
        <el-input v-model="user.name"></el-input>
      </el-form-item>
      <el-form-item label="密码" prop="pass">
        <el-input v-model="user.pass" type="password"></el-input>
      </el-form-item>
      <el-form-item>
        <el-button type="primary" icon="el-icon-upload" @click="login">登录</el-button>
      </el-form-item>
    </el-form>
  </el-row>
</template>
<script lang="ts">
import { Component, Vue, Prop, Watch, Emit } from "vue-property-decorator";
import DialogViewModel from "@/common/viewmodels/dialogViewModel.ts";
import { getModule } from "vuex-module-decorators";
import store from "@/store";
import UserModule from "@/common/stores/userModule";
import api from "../common/network/api.request";

@Component
export default class Login extends Vue {
  module: UserModule = getModule(UserModule, store);
  user: UserViewModel = new UserViewModel();
  rules: any = {
    name: new RuleViewModel("用户名不能为空"),
    pass: new RuleViewModel("密码不能为空")
  };

  get form(): Vue {
    return this.$refs.loginForm as Vue;
  }
  async login() {
    let self: any = this;
    /**
     * Knowledge: Promise 中不管用什么方法都要catch到错误，不然console要出错
     */
    try {
      // 因为validate方法不在Vue里面声明过，要强行返回一个类型
      await (this.form as Vue & {
        validate: () => boolean;
      }).validate();

      var res = await api.get("Login/Token", {
        username: self.user.name,
        password: self.user.pass
      });

      if (!res.succeed) {
        self.$message({
          type: "error",
          message: "用户名或密码错误",
          showClose: true
        });
        return;
      }

      this.module.setToken(res.data);
      console.log(this.module.token);
      self.$notify({
        type: "success",
        message: "欢迎你," + self.user.name + "!",
        duration: 3000
      });
      self.$router.replace("/");
    } catch {}

    // this.form
    //     .validate()
    //     .then(function(value: any) {
    //         var res = await api.get("Login/Token", { username: self.user.name, password: self.user.pass })
    //         if (self.user.name === "admin" && self.user.pass === "123") {
    //             self.$notify({
    //                 type: "success",
    //                 message: "欢迎你," + self.user.name + "!",
    //                 duration: 3000
    //             });
    //             self.$router.replace("/");
    //         } else {
    //             self.$message({
    //                 type: "error",
    //                 message: "用户名或密码错误",
    //                 showClose: true
    //             });
    //         }
    //     })
    //     .catch(function(error: any) {
    //         console.log("login failed: " + error);
    //     });
  }
}

class UserViewModel {
  name: string = "";
  pass: string = "";
}

class RuleViewModel {
  required: boolean = true;
  message: string = "";
  trigger: string = "blur";

  constructor(message: string) {
    this.message = message;
  }
}
</script>