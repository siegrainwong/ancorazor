import { Component, OnInit, Output, EventEmitter } from "@angular/core";
import { OpenIdConnectService } from "src/app/shared/oidc/open-id-connect.service";
import { ActivatedRoute, Router } from "@angular/router";
import { Variables } from "src/app/shared/variables";

@Component({
  selector: "app-nav",
  templateUrl: "./nav.component.html",
  styleUrls: ["./nav.component.scss"]
})
export class NavComponent implements OnInit {
  @Output() toggleSidenav = new EventEmitter<void>();
  title: String;

  constructor(public userService: OpenIdConnectService, variables: Variables) {
    console.log(userService);

    variables.routeDataChanged$.subscribe(data => {
      if (data.kind == "home") this.title = "";
      else
        this.title = `siegrain.wang ${
          userService.userIsAvailable ? ", welcome back!" : ""
        }`;
    });
  }

  ngOnInit() {}
}
