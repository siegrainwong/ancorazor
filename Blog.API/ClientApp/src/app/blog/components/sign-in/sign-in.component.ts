import { Component, OnInit, Input, Inject } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material";
import { UserModel } from "../../models/user-model";
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
    @Inject(MAT_DIALOG_DATA) public data: any,
    public dialogRef: MatDialogRef<SignInComponent>,
    private _service: UserService,
    private _util: SGUtil
  ) {
    this.isReseting = data.isReseting;
  }
  private _passwordValidators = [
    Validators.required,
    Validators.minLength(6),
    Validators.maxLength(30)
  ];

  username = new FormControl("", [
    Validators.required,
    Validators.minLength(4),
    Validators.maxLength(30)
  ]);
  password = new FormControl("", this._passwordValidators);
  newPassword = new FormControl("", this._passwordValidators);
  rePassword = new FormControl(
    "",
    this._passwordValidators.concat(
      MatchValidator.matchControl(this.newPassword)
    )
  );
  @Input() isReseting: boolean = false;
  model: UserModel = new UserModel();
  loading: boolean = false;

  ngOnInit() {}

  cancel() {
    this.dialogRef.close();
    this._util.routeTo(["/"]);
  }

  async submit() {
    this.model.loginName = this.username.value;
    this.model.password = this.password.value;

    this.loading = true;
    let result = await this._service.signIn(
      this.model.loginName,
      this.model.password
    );
    this.loading = false;

    if (!result) return;
    this._util.tip(`Welcome, ${result.realName}`, TipType.Success);
    this.dialogRef.close();
  }

  async reset() {}
}

import { AbstractControl, ValidatorFn } from "@angular/forms";

/**
 * Mark: Custom validator
 * https://angular-templates.io/tutorials/about/angular-forms-and-validations
 */
export class MatchValidator {
  static matchControl = (control: AbstractControl): ValidatorFn => {
    return (matchTo: AbstractControl): { [key: string]: string } => {
      if (control.value == matchTo.value) return null;

      return {
        match: `value not match to ${matchTo.value}`
      };
    };
  };
}
