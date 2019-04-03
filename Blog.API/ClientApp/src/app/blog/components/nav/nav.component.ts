import {
  Component,
  OnInit,
  Output,
  EventEmitter,
  OnDestroy
} from "@angular/core";
import { Store } from "src/app/shared/store/store";
import { SGUtil, TipType } from "src/app/shared/utils/siegrain.utils";
import { SGTransition } from "src/app/shared/utils/siegrain.animations";
import { MatDialog } from "@angular/material";
import { SignInComponent } from "../sign-in/sign-in.component";
import { ActivatedRoute } from "@angular/router";
import { constants } from "src/app/shared/constants/siegrain.constants";
import { Subscription } from "rxjs";

@Component({
  selector: "app-nav",
  templateUrl: "./nav.component.html",
  styleUrls: ["./nav.component.scss"]
})
export class NavComponent implements OnInit, OnDestroy {
  private _subscription = new Subscription();

  title: String;
  navbarOpen: boolean = false;
  items = {
    about: "About",
    newPost: "New Post",
    signIn: "Sign In",
    signOut: "Sign Out",
    reset: "Reset"
  };

  constructor(
    public store: Store,
    public util: SGUtil,
    public transition: SGTransition,
    public dialog: MatDialog,
    private _route: ActivatedRoute
  ) {}

  ngOnInit() {
    this._route.fragment.subscribe(fragment => {
      // 因为生命周期的原因，首屏加载时不能显示dialog
      if (fragment == "sign-in" && !this.store.isFirstScreen)
        this.openDialog(false);
    });
    this.registerRouteChanged();
    if (!this.store.renderFromClient) return;
  }

  ngOnDestroy() {
    this._subscription.unsubscribe();
  }

  onItemTapped(item?: string) {
    switch (item) {
      case this.items.about:
        this.util.routeTo(["/about"]);
        break;
      case this.items.newPost:
        this.util.routeTo(["/add"]);
        break;
      case this.items.signIn:
        this.openDialog(false);
        break;
      case this.items.reset:
        this.openDialog(true);
        break;
      case this.items.signOut:
        this.signOut();
        break;
      default:
        break;
    }
    this.navbarOpen = false;
  }

  openDialog(isReseting: boolean): void {
    this.dialog.open(SignInComponent, {
      width: "300px",
      data: { isReseting: isReseting }
    });
  }

  toggleNavBar() {
    this.navbarOpen = !this.navbarOpen;
  }

  signOut() {
    this.store.user = null;
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
