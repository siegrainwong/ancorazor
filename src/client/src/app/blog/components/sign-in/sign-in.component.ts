import { Component, OnInit, Inject } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material";
import { UserModel } from "../../models/user-model";
import { LoggingService } from "src/app/shared/services/logging.service";
import { FormControl, Validators } from "@angular/forms";
import { UserService } from "../../services/user.service";

@Component({
  selector: "app-sign-in",
  templateUrl: "./sign-in.component.html",
  styleUrls: ["./sign-in.component.scss"]
})
export class SignInComponent implements OnInit {
  constructor(
    public dialogRef: MatDialogRef<SignInComponent>,
    private logger: LoggingService,
    private service: UserService
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

    let result = await this.service.signIn(
      this.model.loginName,
      this.model.password
    );

    if (!result) return;
    this.close();
  }
}
