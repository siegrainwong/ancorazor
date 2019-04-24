import { Component, OnInit } from "@angular/core";
import { constants } from "src/app/shared/constants/siegrain.constants";
import { SGTransitionDelegate } from "src/app/shared/animations/sg-transition.delegate";
import { SGAnimations } from "src/app/shared/animations/sg-animations";

@Component({
  selector: "app-footer",
  templateUrl: "./footer.component.html",
  styleUrls: ["./footer.component.scss"]
})
export class FooterComponent implements OnInit, SGTransitionDelegate {
  copyright = constants.titlePlainText;
  public animations = {
    footer: SGAnimations.fadeOpposite
  };
  constructor() {}

  ngOnInit() {}
}
