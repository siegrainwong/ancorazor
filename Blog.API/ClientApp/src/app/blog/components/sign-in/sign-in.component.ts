import { Component, OnInit, Input, Inject } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material";
import { FormControl, Validators } from "@angular/forms";
import { UserService } from "../../services/user.service";
import { SGUtil, TipType } from "src/app/shared/utils/siegrain.utils";
import { AbstractControl, ValidatorFn } from "@angular/forms";
import { Store } from "src/app/shared/store/store";
import { deriveAKey } from "src/app/shared/utils/pbkdf2.cryptography";
import { Router } from "@angular/router";

@Component({
  selector: "app-sign-in",
  templateUrl: "./sign-in.component.html",
  styleUrls: ["./sign-in.component.scss"]
})
export class SignInComponent implements OnInit {
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    public dialogRef: MatDialogRef<SignInComponent>,
    public store: Store,
    private _service: UserService,
    private _util: SGUtil,
    private _router: Router
  ) {
    this.isReseting = data.isReseting;
  }

  username = new FormControl("", this.getValidator(4));
  password = new FormControl("", this.getValidator(6));
  newPassword = new FormControl("", this.getValidator(6));
  rePassword = new FormControl(
    "",
    this.getValidator(6).concat(MatchValidator.matchTo(this.newPassword))
  );
  @Input() isReseting: boolean = false;

  private getValidator(minLength: number): ValidatorFn[] {
    return [
      Validators.required,
      Validators.minLength(minLength),
      Validators.maxLength(30)
    ];
  }

  ngOnInit() {}

  cancel() {
    this.dialogRef.close();
    if (!this.isReseting) this._router.navigate(["/"]);
  }

  async submit() {
    if (this.username.invalid || this.password.invalid || this.isReseting)
      return;

    let result = await this._service.signIn(
      this.username.value,
      await this.passwordHash(this.password.value)
    );

    if (!result) return;
    this._util.tip(`Welcome, ${result.realName}`, TipType.Success);
    this.dialogRef.close();
  }

  async reset() {
    if (
      this.username.invalid ||
      this.password.invalid ||
      this.newPassword.invalid ||
      this.rePassword.invalid
    )
      return;

    let result = await this._service.reset(
      this.store.user.id,
      this.username.value,
      await this.passwordHash(this.password.value),
      await this.passwordHash(this.newPassword.value)
    );

    if (!result) return;
    this.dialogRef.close();
    this._util.tip(`Reset succeed, sign in again, please.`, TipType.Success);
    this._router.navigate(["/"], { fragment: "sign-in" });
  }

  private async passwordHash(password: string): Promise<string> {
    if (!this.store.renderFromClient) return;
    return await deriveAKey(
      password,
      this.username.value, // salt
      3000, // iterations
      "SHA-512"
    );
  }
}

/**
 * Mark: Custom validator
 * https://angular-templates.io/tutorials/about/angular-forms-and-validations
 */
export class MatchValidator {
  static matchTo = (source: AbstractControl): ValidatorFn => {
    return (target: AbstractControl): { [key: string]: string } => {
      if (source.value == target.value) return null;

      return {
        match: `value not match to ${target.value}`
      };
    };
  };
}
