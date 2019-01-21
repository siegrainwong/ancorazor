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
import FormModule from "@/common/stores/formModule";
import api from "../common/network/api.request";

@Component
export default class Login extends Vue {
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
        this.form
            .validate()
            .then(function(value: any) {
                if (self.user.name === "admin" && self.user.pass === "123") {
                    self.$notify({
                        type: "success",
                        message: "欢迎你," + self.user.name + "!",
                        duration: 3000
                    });
                    self.$router.replace("/");
                } else {
                    self.$message({
                        type: "error",
                        message: "用户名或密码错误",
                        showClose: true
                    });
                }
            })
            .catch(function(error: any) {
                console.log("login failed: " + error);
            });
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