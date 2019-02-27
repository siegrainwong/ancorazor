import { Component, OnInit, Input, EventEmitter, Output } from "@angular/core";
import ArticleModel from "../../models/article-model";
import { Variables } from "src/app/shared/variables";
import { random } from "src/app/shared/utils/siegrain.utils";
import * as $ from "jquery";

@Component({
  selector: "app-header",
  templateUrl: "./header.component.html",
  styleUrls: ["./header.component.scss"]
})
export class HeaderComponent implements OnInit {
  @Input() model: ArticleModel = new ArticleModel();
  @Input() isEditing: boolean = false;
  @Output() modelChanged = new EventEmitter<ArticleModel>();

  constructor(private variables: Variables) {}

  ngOnInit() {
    this.registerRouteChanged();
  }

  registerRouteChanged() {
    this.variables.routeDataChanged$.subscribe(data => {
      if (this.variables.renderFromServer) this.model.cover = "";
      if (this.model.cover || this.variables.renderFromServer) return;
      switch (data.kind) {
        case "article":
          this.model.cover = "assets/img/post-bg.jpg";
          break;
        case "add":
          this.model.cover = "assets/img/home-bg.jpg";
          break;
        default:
          this.model.cover = `assets/img/bg${random(1, 7)}.jpg`;
          break;
      }

      let image = new Image();
      image.onload = function() {
        $(".masthead").css("background-image", "url('" + image.src + "')");
      };
      image.src = this.model.cover;
    });
  }

  onTitleBlured(val: string) {
    this.model.title = val;
    this.modelChanged.emit(this.model);
  }
  onDigestBlured(val: string) {
    this.model.digest = val;
    this.modelChanged.emit(this.model);
  }
}
