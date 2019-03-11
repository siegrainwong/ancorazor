import { Component, Inject } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material";
import { TipType } from "../utils/siegrain.utils";

export class ConfirmDialogData {
  title: string = "Confirm";
  content?: string;
  confirmed: boolean = false;
  type: TipType = TipType.Confirm;

  constructor(obj?: Partial<ConfirmDialogData>) {
    Object.assign(this, obj);
  }
}
@Component({
  selector: "dialog-data-example-dialog",
  template: `
    <h1 mat-dialog-title>{{ data.type + " " + data.title }}</h1>
    <div mat-dialog-content *ngIf="data.content">{{ data.content }}</div>
    <div style="margin-top:20px;">
      <button class="float-left mr-2" mat-button [mat-dialog-close]="false">
        No
      </button>
      <button
        mat-button
        [color]="color"
        class="float-right"
        [mat-dialog-close]="true"
        cdkFocusInitial
      >
        Yes
      </button>
    </div>
  `
})
export class ConfirmDialog {
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: ConfirmDialogData,
    public dialogRef: MatDialogRef<ConfirmDialog>
  ) {}
  close() {
    this.dialogRef.close();
  }

  get color() {
    switch (this.data.type) {
      case TipType.Danger:
        return "danger";
      case TipType.Warn:
        return "warn";
      default:
        return "primary";
    }
  }
}
