import { Component, HostBinding } from "@angular/core";
import { slideInAnimation } from "./shared/utils/animations";
import { RouterOutlet } from "@angular/router";

@Component({
  selector: "app-root",
  template: `
    <router-outlet></router-outlet>
  `
  // templateUrl: "./app.component.html"
})
export class AppComponent {}
