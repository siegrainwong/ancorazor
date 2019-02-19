import { Component } from "@angular/core";
import { OpenIdConnectService } from "./shared/oidc/open-id-connect.service";

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.scss"]
})
export class AppComponent {
  title = "client";
  constructor(private userService: OpenIdConnectService) {
    // if (!userService.userIsAvailable) userService.triggerSignIn();
  }
}
