import { Component, OnInit, Input, EventEmitter, Output } from "@angular/core";
import ArticleModel from "../../models/article-model";
import { Store } from "src/app/shared/store/store";
import * as $ from "jquery";
import { SGTransition } from "src/app/shared/utils/siegrain.animations";
@Component({
  selector: "app-header",
  templateUrl: "./header.component.html",
  styleUrls: ["./header.component.scss"]
})
export class HeaderComponent implements OnInit {
  private _model: ArticleModel;

  @Input() isEditing: boolean = false;
  // 给 write-article 页面用的
  @Output() headerUpdated = new EventEmitter<ArticleModel>();

  constructor(private store: Store, public transition: SGTransition) {}

  ngOnInit() {}

  get model() {
    return this._model;
  }

  @Input() set model(val) {
    this._model = val;
    this.loadCover();
  }

  loadCover() {
    if (this.store.renderFromServer) return;
    let src = (this.model && this.model.cover) || "assets/img/placeholder.jpg";
    let image = new Image();
    image.onload = function() {
      $(`.header-bg`).css("background-image", "url('" + image.src + "')");
    };
    image.src = src;
  }

  onTitleChanged(val: string) {
    this.model.title = val;
    this.headerUpdated.emit(this.model);
  }
  onDigestChanged(val: string) {
    this.model.digest = val;
    this.headerUpdated.emit(this.model);
  }
}
