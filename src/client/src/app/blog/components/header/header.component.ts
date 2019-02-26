import { Component, OnInit, Input, EventEmitter, Output } from "@angular/core";
import ArticleModel from "../../models/article-model";

@Component({
  selector: "app-header",
  templateUrl: "./header.component.html",
  styleUrls: ["./header.component.scss"]
})
export class HeaderComponent implements OnInit {
  @Input() model: ArticleModel = new ArticleModel();
  @Input() isEditing: boolean = false;
  @Output() modelChanged = new EventEmitter<ArticleModel>();

  constructor() {}

  ngOnInit() {
    if (!this.model.cover) this.model.cover = "assets/img/home-bg.jpg";
  }

  onTitleBlured(val) {
    this.model.title = val;
    this.modelChanged.emit(this.model);
  }
  onDigestBlured(val) {
    this.model.digest = val;
    this.modelChanged.emit(this.model);
  }
}
