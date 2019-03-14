import { Component, OnInit, Output, EventEmitter } from "@angular/core";
import { Store } from "src/app/shared/store/store";
import { environment } from "src/environments/environment";
import { SGUtil, TipType } from "src/app/shared/utils/siegrain.utils";
import { SGTransition } from "src/app/shared/utils/siegrain.animations";
import { MatDialog } from "@angular/material";
import { SignInComponent } from "../sign-in/sign-in.component";
import { ActivatedRoute } from "@angular/router";

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
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.route.fragment.subscribe(fragment => {
      if (fragment == "sign-in") this.openDialog();
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
      else this.title = environment.title;
    });
  }
}
