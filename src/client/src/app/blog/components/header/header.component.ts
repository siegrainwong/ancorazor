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
      if (this.model.cover || this.variables.renderFromServer) return;
      switch (data.kind) {
        case "article":
          this.model.cover = "assets/img/article-bg.jpg";
          break;
        case "add":
          this.model.cover = "assets/img/write-bg.jpg";
          break;
        case "home":
          // 同一session下保持同一张cover比较好
          if (this.variables.homeCover) {
            this.model.cover = this.variables.homeCover;
            this.loadCover(false, this.model.cover);
            return;
          } else {
            this.model.cover = `assets/img/bg${random(1, 7)}.jpg`;
            this.variables.homeCover = this.model.cover;
          }
          break;
        default:
          this.model.cover = "assets/img/article-bg.jpg";
          break;
      }

      this.loadCover(true, this.model.cover);
    });
  }

  loadCover(shouldTransition: boolean, src: string) {
    if (shouldTransition) {
      let image = new Image();
      image.onload = function() {
        $(".masthead")
          .css("transition", "background-image 3s")
          .css("background-image", "url('" + image.src + "')");
      };
      image.src = src;
    } else {
      $(".masthead")
        .css("transition", "none")
        .css("background-image", "url('" + src + "')");
    }
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
