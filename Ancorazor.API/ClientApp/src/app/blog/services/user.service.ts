import { Injectable, OnInit } from "@angular/core";
import { BaseService, ISGService } from "src/app/shared/services/base.service";
import { UserModel } from "../models/user-model";
import { ResponseResult } from "src/app/shared/models/response-result";

@Injectable({ providedIn: "root" })
export class UserService extends BaseService implements ISGService {
  protected initialize() {}
  disabledCache = () => true;
  serviceName = "Users";

  /**
   * 登录
   */
  async signIn(loginname: string, password: string): Promise<UserModel> {
    let res = await this.post(`${this.serviceName}/SignIn`, {
      loginname: loginname,
      password: password
    });
    if (!res || !res.succeed) return null;
    let model = res.data.user as UserModel;
    this.store.signIn(model);
    return model;
  }

  /**
   * 注销
   */
  async signOut(): Promise<boolean> {
    let res = await this.post(`${this.serviceName}/SignOut`, null);
    if (!res || !res.succeed) return false;
    this.store.signOut();
  }

  /**
   * 重置密码
   */
  async reset(
    id: number,
    username: string,
    password: string,
    newPassword: string
  ): Promise<boolean> {
    let res = await this.put(`${this.serviceName}/Reset`, {
      id: id,
      loginName: username,
      password: password,
      newPassword: newPassword
    });
    if (res && res.succeed) {
      this.store.signOut();
      return true;
    }
    return false;
  }

  /**
   * 在切换凭证时重置`XSRFToken`
   */
  async getXSRFToken() {
    let res = await this.get(`${this.serviceName}/XSRFToken`);
    if (res && res.succeed) this.logger.info("XSRFToken has been reset.");
    else this.logger.warn("XSRFToken reset failed!");
  }

  handleError(result: ResponseResult, code?: number): ResponseResult {
    switch (code) {
      case 403:
        this.util.tip("Invalid username or password");
        break;
      default:
        return super.handleError(result, code);
    }
    return new ResponseResult(result);
  }
}
