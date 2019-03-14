import { Component, HostBinding } from "@angular/core";
import { slideInAnimation } from "./shared/utils/animations.ng";

@Component({
  selector: "app-root",
  template: `
    <router-outlet></router-outlet>
  `
  // templateUrl: "./app.component.html"
})
export class AppComponent {}
