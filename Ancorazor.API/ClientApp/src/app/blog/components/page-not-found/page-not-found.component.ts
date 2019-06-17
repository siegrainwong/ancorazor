import { Component, OnInit } from "@angular/core";
import { environment } from "src/environments/environment";
import { SGTransitionDelegate } from "src/app/shared/animations/sg-transition.delegate";
import { SGAnimations } from "src/app/shared/animations/sg-animations";

@Component({
  selector: "app-page-not-found",
  templateUrl: "./page-not-found.component.html",
  styleUrls: ["./page-not-found.component.scss"]
})
export class PageNotFoundComponent implements OnInit, SGTransitionDelegate {
  public animations = {
    default: SGAnimations.fadeOpposite
  };
  public home = environment.host;

  ngOnInit() {}
}
