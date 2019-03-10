import { Injectable } from "@angular/core";
import { BaseService, ISubService } from "src/app/shared/services/base.service";
import { UserModel } from "../models/user-model";

@Injectable({
  providedIn: "root"
})
export class UserService extends BaseService implements ISubService {
  serviceName = "Users";

  /**
   * 登录
   */
  async signIn(loginname: string, password: string): Promise<boolean> {
    let res = await this.get(`${this.serviceName}/Token`, {
      loginname: loginname,
      password: password
    });
    if (!res || !res.succeed) return false;
    let model = res.data.user as UserModel;
    model.token = res.data.token;
    this.store.user = model;
    return true;
  }
}
