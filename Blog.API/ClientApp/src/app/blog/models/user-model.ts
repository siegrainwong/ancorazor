import BaseModel from "src/app/shared/models/base-model";

export class UserModel extends BaseModel {
  loginName: string = "";
  password: string = "";
  realName: string;
  // token: string;

  constructor(init?: Partial<UserModel>) {
    super(init);
    Object.assign(this, init);
  }
}
