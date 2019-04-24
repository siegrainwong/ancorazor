import { Component, OnInit, Input, EventEmitter, Output } from "@angular/core";
import ArticleModel from "../../models/article-model";
import { Store } from "src/app/shared/store/store";
import { SGTransition } from "src/app/shared/animations/sg-transition";
import { SGTransitionDelegate } from "src/app/shared/animations/sg-transition.delegate";
import { SGAnimations } from "src/app/shared/animations/sg-animations";
@Component({
  selector: "app-header",
  templateUrl: "./header.component.html",
  styleUrls: ["./header.component.scss"]
})
export class HeaderComponent implements SGTransitionDelegate {
  public animations = {
    cover: SGAnimations.fade,
    title: SGAnimations.fadeUp
  };

  @Input() isEditing: boolean = false;
  @Output() headerUpdated = new EventEmitter<ArticleModel>();

  constructor(private _store: Store, public transition: SGTransition) {}
  private _model: ArticleModel;
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
