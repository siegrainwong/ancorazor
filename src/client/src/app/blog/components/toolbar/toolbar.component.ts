import { Component, OnInit, Output, EventEmitter } from "@angular/core";
// import { OpenIdConnectService } from "src/app/shared/oidc/open-id-connect.service";

@Component({
  selector: "app-toolbar",
  templateUrl: "./toolbar.component.html",
  styleUrls: ["./toolbar.component.scss"]
})
export class ToolbarComponent implements OnInit {
  @Output() toggleSidenav = new EventEmitter<void>();

  public userService = null;
  constructor() // public userService: OpenIdConnectService
  {}

  ngOnInit() {}
}
