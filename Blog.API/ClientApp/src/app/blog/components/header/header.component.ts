import { Component, OnInit, Input, EventEmitter, Output } from "@angular/core";
import ArticleModel from "../../models/article-model";
import { Store } from "src/app/shared/store/store";
import { SGTransition } from "src/app/shared/utils/siegrain.animations";
@Component({
  selector: "app-header",
  templateUrl: "./header.component.html",
  styleUrls: ["./header.component.scss"]
})
export class HeaderComponent implements OnInit {
  private _model: ArticleModel;

  @Input() isEditing: boolean = false;
  @Output() headerUpdated = new EventEmitter<ArticleModel>();

  constructor(private _store: Store, public transition: SGTransition) {}

  ngOnInit() {}

  get model() {
    return this._model;
  }

  @Input() set model(val) {
    this._model = val;
    if (this._store.renderFromClient) this.loadCover();
  }

  loadCover() {
    let src = (this.model && this.model.cover) || "assets/img/placeholder.jpg";
    let image = new Image();
    image.onload = function() {
      (document.querySelector(
        ".header-bg"
      ) as HTMLElement).style.backgroundImage = "url('" + image.src + "')";
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
