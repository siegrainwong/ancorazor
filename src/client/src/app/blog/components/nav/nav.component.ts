import { Component, OnInit, Output, EventEmitter } from "@angular/core";
// import { OpenIdConnectService } from "src/app/shared/oidc/open-id-connect.service";
import { Store } from "src/app/shared/store/store";
import { environment } from "src/environments/environment";
import { SGUtil, TipType } from "src/app/shared/utils/siegrain.utils";
import { SGTransition } from "src/app/shared/utils/siegrain.animations";
import { MatDialog } from "@angular/material";
import { SignInComponent } from "../sign-in/sign-in.component";

@Component({
  selector: "app-nav",
  templateUrl: "./nav.component.html",
  styleUrls: ["./nav.component.scss"]
})
export class NavComponent implements OnInit {
  @Output() toggleSidenav = new EventEmitter<void>();
  title: String;

  constructor(
    // public userService: OpenIdConnectService,
    public store: Store,
    public util: SGUtil,
    public transition: SGTransition,
    public dialog: MatDialog
  ) {}

  ngOnInit() {
    this.registerRouteChanged();
  }

  openDialog(): void {
    this.dialog.open(SignInComponent, { width: "250px" });
  }

  signOut() {
    this.store.user = null;
    this.util.tip("已注销", TipType.Success);
  }

  registerRouteChanged() {
    this.store.routeDataChanged$.subscribe(data => {
      if (data && data.kind == "home") this.title = "";
      else this.title = environment.title;
    });
  }
}
