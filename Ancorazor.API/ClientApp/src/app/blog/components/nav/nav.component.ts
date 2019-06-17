import { Component, OnInit, OnDestroy, Input } from "@angular/core";
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
import { UserService } from "../../services/user.service";
import { filter } from "rxjs/operators";
import { SGTransitionDelegate } from "src/app/shared/animations/sg-transition.delegate";
import { SGAnimations } from "src/app/shared/animations/sg-animations";
import { SGRouteTransitionCommands } from "src/app/shared/animations/sg-transition.model";
import { ObservedComponentBase } from "src/app/shared/components/observed.base";
import { AutoUnsubscribe } from "src/app/shared/utils/auto-unsubscribe.decorator";
import { RouteKinds } from "src/app/shared/models/route-data.model";

const fixedPages = [RouteKinds.edit, RouteKinds.notfound, RouteKinds.add];

@Component({
  selector: "app-nav",
  templateUrl: "./nav.component.html",
  styleUrls: ["./nav.component.scss"]
})
@AutoUnsubscribe()
export class NavComponent extends ObservedComponentBase
  implements OnInit, OnDestroy, SGTransitionDelegate {
  public animations = {
    title: SGAnimations.fade
  };
  public title: String;
  public navbarOpen: boolean = false;
  public alwaysFixed: boolean = false;

  private _routeChanged$;
  private _settingChanged$;
  private _fragmentChanged$;
  constructor(
    public store: Store,
    public util: SGUtil,
    public transition: SGTransitionToEnter,
    public dialog: MatDialog,
    private _route: ActivatedRoute,
    private _service: UserService
  ) {
    super();
  }

  transitionForComponent(nextRoute: ActivatedRouteSnapshot) {
    return new SGRouteTransitionCommands({ scrollTo: topElementId });
  }

  ngOnInit() {
    this.registerSubscriptions();
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
    this._routeChanged$ = this.store.routeDataChanged$.subscribe(data => {
      if (data.kind == RouteKinds.home || data.kind == RouteKinds.homePaged)
        this.title = "";
      else this.title = this.store.siteSetting && this.store.siteSetting.title;

      this.alwaysFixed = fixedPages.indexOf(data.kind as RouteKinds) > -1;
    });
    this._settingChanged$ = this.store.siteSettingChanged$.subscribe(data => {
      const isHomePage = this.title === "";
      if (!isHomePage) this.title = data.title;
    });
    // 因为生命周期的原因，首屏加载时不能显示dialog
    this._fragmentChanged$ = this._route.fragment
      .pipe(filter(f => f == "sign-in" && !this.store.isFirstScreen))
      .subscribe(_ => this.openDialog(false));
  }
}
