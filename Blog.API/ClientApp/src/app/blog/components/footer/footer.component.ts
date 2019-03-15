import { Component, OnInit } from "@angular/core";
import { constants } from "src/app/shared/constants/siegrain.constants";

@Component({
  selector: "app-footer",
  templateUrl: "./footer.component.html",
  styleUrls: ["./footer.component.scss"]
})
export class FooterComponent implements OnInit {
  copyright = constants.titlePlainText;
  constructor() {}

  ngOnInit() {}
}
