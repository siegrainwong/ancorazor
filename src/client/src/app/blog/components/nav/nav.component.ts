import { Component, OnInit, Output, EventEmitter } from "@angular/core";
import { OpenIdConnectService } from "src/app/shared/oidc/open-id-connect.service";
import { Variables } from "src/app/shared/variables";
import { environment } from "src/environments/environment";

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
    private variables: Variables
  ) {}

  ngOnInit() {
    this.registerRouteChanged();
  }

  registerRouteChanged() {
    this.variables.routeDataChanged$.subscribe(data => {
      if (data && data.kind == "home") this.title = "";
      else this.title = environment.title;
    });
  }
}
