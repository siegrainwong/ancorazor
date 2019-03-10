import { Component, OnInit, Inject } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material";
import { UserModel } from "../../models/user-model";

@Component({
  selector: "app-sign-in",
  templateUrl: "./sign-in.component.html",
  styleUrls: ["./sign-in.component.scss"]
})
export class SignInComponent implements OnInit {
  constructor(public dialogRef: MatDialogRef<SignInComponent>, @Inject(MAT_DIALOG_DATA) public data: any) {}
  model: UserModel = new UserModel();

  ngOnInit() {}

  submit() {}
}
