import { Component, OnInit, OnDestroy } from "@angular/core";
import { Store } from "src/app/shared/store/store";
import { SGUtil, TipType } from "src/app/shared/utils/siegrain.utils";
import { SGTransition } from "src/app/shared/animations/sg-transition";
import { MatDialog } from "@angular/material";
import { SignInComponent } from "../sign-in/sign-in.component";
import { ActivatedRoute } from "@angular/router";
import { constants } from "src/app/shared/constants/siegrain.constants";
import { Subscription } from "rxjs";
import { UserService } from "../../services/user.service";
import { filter } from "rxjs/operators";
import { SGTransitionDelegate } from "src/app/shared/animations/sg-transition.delegate";
import { SGAnimations } from "src/app/shared/animations/sg-animations";

@Component({
  selector: "app-nav",
  templateUrl: "./nav.component.html",
  styleUrls: ["./nav.component.scss"]
})
export class NavComponent implements OnInit, OnDestroy, SGTransitionDelegate {
  public animations = {
    title: SGAnimations.fade
  };
  private _subscription = new Subscription();

  title: String;
  navbarOpen: boolean = false;
  constructor(
    public store: Store,
    public util: SGUtil,
    public transition: SGTransition,
    public dialog: MatDialog,
    private _route: ActivatedRoute,
    private _service: UserService
  ) {}

  ngOnInit() {
    // 因为生命周期的原因，首屏加载时不能显示dialog
    this._subscription.add(
      this._route.fragment
        .pipe(filter(f => f == "sign-in" && !this.store.isFirstScreen))
        .subscribe(_ => this.openDialog(false))
    );
    this.registerRouteChanged();
    if (!this.store.renderFromClient) return;
  }

  ngOnDestroy() {
    this._subscription.unsubscribe();
  }

  openDialog(isReseting: boolean): void {
    this.dialog.open(SignInComponent, {
      width: "300px",
      data: { isReseting: isReseting }
    });
    this.navbarOpen = false;
  }

  toggleNavBar() {
    this.navbarOpen = !this.navbarOpen;
  }

  async signOut() {
    (await this._service.signOut()) &&
      this.util.tip("Signed out", TipType.Success);
  }

  registerRouteChanged() {
    this._subscription.add(
      this.store.routeDataChanged$.subscribe(data => {
        if (data && data.kind == "home") this.title = "";
        else this.title = constants.title;
      })
    );
  }
}
