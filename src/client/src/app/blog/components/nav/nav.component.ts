import {
  Component,
  OnInit,
  Output,
  EventEmitter,
  HostBinding
} from "@angular/core";
import { OpenIdConnectService } from "src/app/shared/oidc/open-id-connect.service";
import { Store } from "src/app/shared/store/store";
import { environment } from "src/environments/environment";
import { Router } from "@angular/router";
import { timeout } from "src/app/shared/utils/siegrain.utils";

@Component({
  selector: "app-nav",
  templateUrl: "./nav.component.html",
  styleUrls: ["./nav.component.scss"]
})
export class NavComponent implements OnInit {
  @Output() toggleSidenav = new EventEmitter<void>();
  title: String;

  constructor(
    public userService: OpenIdConnectService,
    public store: Store,
    private router: Router
  ) {}

  ngOnInit() {
    this.registerRouteChanged();
  }

  registerRouteChanged() {
    this.store.routeDataChanged$.subscribe(data => {
      if (data && data.kind == "home") this.title = "";
      else this.title = environment.title;
    });
  }

  async routeTo(url: string) {
    this.store.isLeaving = true;
    await timeout(800);
    this.store.isLeaving = false;
    this.router.navigate([url]);
  }
}
