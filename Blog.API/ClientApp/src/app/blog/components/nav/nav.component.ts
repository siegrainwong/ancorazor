import { Component, OnInit, OnDestroy } from "@angular/core";
import { Store } from "src/app/shared/store/store";
import {
  SGUtil,
  TipType,
  topElementId
} from "src/app/shared/utils/siegrain.utils";
import { SGTransitionToEnter } from "src/app/shared/animations/sg-transition.enter";
import { MatDialog } from "@angular/material";
import { SignInComponent } from "../sign-in/sign-in.component";
import { ActivatedRoute, ActivatedRouteSnapshot } from "@angular/router";
import { constants } from "src/app/shared/constants/siegrain.constants";
import { Subscription } from "rxjs";
import { UserService } from "../../services/user.service";
import { filter } from "rxjs/operators";
import { SGTransitionDelegate } from "src/app/shared/animations/sg-transition.delegate";
import { SGAnimations } from "src/app/shared/animations/sg-animations";
import { SGRouteTransitionCommands } from "src/app/shared/animations/sg-transition.model";

@Component({
  selector: "app-nav",
  templateUrl: "./nav.component.html",
  styleUrls: ["./nav.component.scss"]
})
export class NavComponent implements OnInit, OnDestroy, SGTransitionDelegate {
  public animations = {
    title: SGAnimations.fade
  };
  public title: String;
  public navbarOpen: boolean = false;

  private _subscription = new Subscription();
  constructor(
    public store: Store,
    public util: SGUtil,
    public transition: SGTransitionToEnter,
    public dialog: MatDialog,
    private _route: ActivatedRoute,
    private _service: UserService
  ) {}

  transitionForComponent(nextRoute: ActivatedRouteSnapshot) {
    return new SGRouteTransitionCommands({ scrollTo: topElementId });
  }

  ngOnInit() {
    this.registerSubscriptions();
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

  registerSubscriptions() {
    this._subscription.add(
      this.store.routeDataChanged$.subscribe(data => {
        if (data && data.kind == "home") this.title = "";
        else
          this.title = this.store.siteSetting && this.store.siteSetting.title;
      })
    );
    this._subscription.add(
      this.store.siteSettingChanged$.subscribe(data => {
        const isHomePage = this.title === "";
        if (!isHomePage) this.title = data.title;
      })
    );
    this._subscription.add(
      // 因为生命周期的原因，首屏加载时不能显示dialog
      this._route.fragment
        .pipe(filter(f => f == "sign-in" && !this.store.isFirstScreen))
        .subscribe(_ => this.openDialog(false))
    );
  }
}
