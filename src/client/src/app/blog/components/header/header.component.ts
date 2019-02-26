import { Component, OnInit, Input, EventEmitter, Output } from "@angular/core";
import ArticleModel from "../../models/article-model";
import { Variables } from "src/app/shared/variables";

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
      if (this.model.cover) return;
      switch (data.kind) {
        case "article":
          this.model.cover = "assets/img/post-bg.jpg";
          break;
        default:
          this.model.cover = "assets/img/home-bg.jpg";
          break;
      }
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
