import { Component, OnInit, Output, EventEmitter } from "@angular/core";
import { OpenIdConnectService } from "src/app/shared/oidc/open-id-connect.service";

@Component({
  selector: "app-nav",
  templateUrl: "./nav.component.html",
  styleUrls: ["./nav.component.scss"]
})
export class NavComponent implements OnInit {
  @Output() toggleSidenav = new EventEmitter<void>();

  constructor(public userService: OpenIdConnectService) {
    console.log(userService);
  }

  ngOnInit() {}
}
