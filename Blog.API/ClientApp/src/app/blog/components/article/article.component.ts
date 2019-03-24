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
  externalScripts
} from "src/app/shared/constants/siegrain.constants";
import { SGUtil } from "src/app/shared/utils/siegrain.utils";

@Component({
  selector: "app-article",
  templateUrl: "./article.component.html",
  styleUrls: ["./article.component.scss"]
})
export class ArticleComponent implements OnInit {
  model: ArticleModel;
  content: string;
  constructor(
    private _service: ArticleService,
    private _route: ActivatedRoute,
    private _titleService: Title,
    private _util: SGUtil,
    public store: Store,
    public transition: SGTransition
  ) {}
  async ngOnInit() {
    await this.getArticle();
    this.setupEditor();
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
    return this.transition.apply("fade-opposite");
  }

  async setupEditor() {
    await this._util.loadExternalScripts([externalScripts.highlight]);
    const md = require("markdown-it")({
      highlight: function(str, lang) {
        if (lang && hljs.getLanguage(lang)) {
          try {
            return (
              '<pre class="hljs"><code class="hljs-code">' +
              hljs.highlight(lang, str, true).value +
              "</code></pre>"
            );
          } catch (__) {}
        }
        return (
          '<pre class="hljs"><code class="hljs-code">' +
          md.utils.escapeHtml(str) +
          "</code></pre>"
        );
      }
    });
    var result = md.render(this.model.content);
    this.content = result;
  }
}
