import { Component, OnInit, ViewEncapsulation } from "@angular/core";
import ArticleModel from "../../models/article-model";
import { ArticleService } from "../../services/article.service";
import { ActivatedRoute } from "@angular/router";
import { Store } from "src/app/shared/store/store";
import { SGTransition } from "src/app/shared/utils/siegrain.animations";
import { Title } from "@angular/platform-browser";
import { timeout } from "src/app/shared/utils/promise-delay";
import {
  constants,
  externalScripts} from "src/app/shared/constants/siegrain.constants";
import { SGUtil } from "src/app/shared/utils/siegrain.utils";

@Component({
  selector: "app-article",
  templateUrl: "./article.component.html",
  styleUrls: ["./article.component.scss"]
})
export class ArticleComponent implements OnInit {
  model: ArticleModel;
  constructor(
    private _service: ArticleService,
    private _route: ActivatedRoute,
    public store: Store,
    private _transition: SGTransition,
    private _titleService: Title,
    private _util: SGUtil
  ) {}
  async ngOnInit() {
    await this.getArticle();
    if (this.store.renderFromClient) this.setupEditor();
  }

  private async getArticle() {
    if (this.store.preloadArticle) {
      this.model = this.store.preloadArticle;
      this.store.preloadArticle = null;
    } else {
      let id = parseInt(this._route.snapshot.params.id);
      let res = await this._service.getArticle(id);
      if (!res) return;
      this.model = res;
    }
    await timeout(10); // 这里必须要 await 一下，给 angular render 的时间
    this._titleService.setTitle(
      `${this.model.title} - ${constants.titlePlainText}`
    );
  }

  get transitionClass() {
    return this.model && this._transition.apply("fade-opposite");
  }

  async setupEditor() {
    await this._util.loadExternalScripts(externalScripts.tuiEditor);
    new tui.Editor.factory({
      el: document.querySelector("#viewer"),
      viewer: true,
      initialValue: this.model.content
    });
  }
}
