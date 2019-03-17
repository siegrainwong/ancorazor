import { Component, OnInit, Output, EventEmitter } from "@angular/core";
import { Store } from "src/app/shared/store/store";
import { SGUtil, TipType } from "src/app/shared/utils/siegrain.utils";
import { SGTransition } from "src/app/shared/utils/siegrain.animations";
import { MatDialog } from "@angular/material";
import { SignInComponent } from "../sign-in/sign-in.component";
import { ActivatedRoute } from "@angular/router";
import { constants } from "src/app/shared/constants/siegrain.constants";

@Component({
  selector: "app-nav",
  templateUrl: "./nav.component.html",
  styleUrls: ["./nav.component.scss"]
})
export class NavComponent implements OnInit {
  @Output() toggleSidenav = new EventEmitter<void>();
  title: String;

  constructor(
    public store: Store,
    public util: SGUtil,
    public transition: SGTransition,
    public dialog: MatDialog,
    private _route: ActivatedRoute
  ) {}

  ngOnInit() {
    this._route.fragment.subscribe(fragment => {
      // 因为生命周期的原因，这个地方还不能弹出dialog
      if (fragment == "sign-in" && !this.store.isFirstScreen) this.openDialog();
    });
    this.registerRouteChanged();
  }

  openDialog(): void {
    this.dialog.open(SignInComponent, { width: "250px" });
  }

  signOut() {
    this.store.user = null;
    this.util.tip("Signed out", TipType.Success);
  }

  registerRouteChanged() {
    this.store.routeDataChanged$.subscribe(data => {
      if (data && data.kind == "home") this.title = "";
      else this.title = constants.title;
    });
  }
}
