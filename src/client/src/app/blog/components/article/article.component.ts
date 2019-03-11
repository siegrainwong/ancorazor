import { Component, OnInit } from "@angular/core";
import ArticleModel from "../../models/article-model";
import { ArticleService } from "../../services/article.service";
import { ActivatedRoute } from "@angular/router";
import { Store } from "src/app/shared/store/store";
import { SGTransition } from "src/app/shared/utils/siegrain.animations";
import { Title } from "@angular/platform-browser";
import { environment } from "src/environments/environment";
import { timeout } from "src/app/shared/utils/promise-delay";

@Component({
  selector: "app-article",
  templateUrl: "./article.component.html",
  styleUrls: ["./article.component.scss"]
})
export class ArticleComponent implements OnInit {
  model: ArticleModel;
  constructor(
    private service: ArticleService,
    private route: ActivatedRoute,
    public store: Store,
    private transition: SGTransition,
    private titleService: Title
  ) {}
  ngOnInit() {
    this.getArticle();
  }

  async getArticle() {
    if (this.store.preloadArticle) {
      await timeout(10); // 这里必须要 await 一下，不然下面内容加载不出来。
      this.model = this.store.preloadArticle;
      this.store.preloadArticle = null;
    } else {
      let id = parseInt(this.route.snapshot.params.id);
      let res = await this.service.getArticle(id);
      if (!res) return;
      this.model = res;
    }

    this.titleService.setTitle(
      `${this.model.title} - ${environment.titlePlainText}`
    );
    this.setupEditor();
  }

  get transitionClass() {
    return this.model && this.transition.apply("article");
  }

  setupEditor() {
    if (this.store.renderFromServer) return;
    let Viewer = require("tui-editor/dist/tui-editor-Viewer");
    new Viewer({
      el: document.querySelector("#viewer"),
      initialValue: this.model.content
    });
  }
}
