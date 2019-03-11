import { Component, OnInit } from "@angular/core";
import { MatDialogRef } from "@angular/material";
import { UserModel } from "../../models/user-model";
import { LoggingService } from "src/app/shared/services/logging.service";
import { FormControl, Validators } from "@angular/forms";
import { UserService } from "../../services/user.service";
import { SGUtil, TipType } from "src/app/shared/utils/siegrain.utils";

@Component({
  selector: "app-sign-in",
  templateUrl: "./sign-in.component.html",
  styleUrls: ["./sign-in.component.scss"]
})
export class SignInComponent implements OnInit {
  constructor(
    public dialogRef: MatDialogRef<SignInComponent>,
    private logger: LoggingService,
    private service: UserService,
    private util: SGUtil
  ) {}
  username = new FormControl("", [
    Validators.required,
    Validators.minLength(4),
    Validators.maxLength(30)
  ]);
  password = new FormControl("", [
    Validators.required,
    Validators.minLength(6),
    Validators.maxLength(30)
  ]);
  model: UserModel = new UserModel();
  loading: boolean = false;

  ngOnInit() {}

  getErrorMessage() {
    if (this.username.errors && !this.username.errors.required)
      return "Username must be 4-30 characters";
    if (this.password.errors && !this.password.errors.required)
      return "Password must be 6-30 characters";
    return "required";
  }

  close() {
    this.dialogRef.close();
  }

  async submit() {
    if (this.username.invalid || this.password.invalid) return;

    this.model.loginName = this.username.value;
    this.model.password = this.password.value;

    this.loading = true;
    let result = await this.service.signIn(
      this.model.loginName,
      this.model.password
    );
    this.loading = false;

    if (!result) return;
    this.util.tip("登录成功。", TipType.Success);
    this.close();
  }
}
