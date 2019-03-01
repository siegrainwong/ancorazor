import { Component, OnInit } from "@angular/core";
import { OpenIdConnectService } from "../open-id-connect.service";
import { Store } from "../../store/store";

@Component({
  selector: "app-redirect-silent-renew",
  templateUrl: "./redirect-silent-renew.component.html",
  styleUrls: ["./redirect-silent-renew.component.scss"]
})
export class RedirectSilentRenewComponent implements OnInit {
  constructor(
    private openIdConnectService: OpenIdConnectService,
    private variebles: Store
  ) {}

  ngOnInit() {
    if (this.variebles.renderFromServer) return;
    this.openIdConnectService.handleSilentCallback();
  }
}
