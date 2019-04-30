import { Component, OnInit, OnDestroy } from "@angular/core";
import ArticleModel from "../../models/article-model";
import { ActivatedRoute } from "@angular/router";
import { Store } from "src/app/shared/store/store";
import { Title } from "@angular/platform-browser";
import { timeout } from "src/app/shared/utils/promise-delay";
import {
  constants,
  externalScripts
} from "src/app/shared/constants/siegrain.constants";
import { SGUtil } from "src/app/shared/utils/siegrain.utils";
import { Subscription } from "rxjs";
import { SGTransitionDelegate } from "src/app/shared/animations/sg-transition.delegate";
import { SGAnimations } from "src/app/shared/animations/sg-animations";

@Component({
  selector: "app-article",
  templateUrl: "./article.component.html",
  styleUrls: ["./article.component.scss"]
})
export class ArticleComponent
  implements OnInit, OnDestroy, SGTransitionDelegate {
  private _subscription = new Subscription();
  public animations = {
    content: SGAnimations.fade
  };
  public model: ArticleModel;
  public content: string;
  constructor(
    private _route: ActivatedRoute,
    private _titleService: Title,
    private _util: SGUtil,
    public store: Store
  ) {}
  ngOnInit() {
    this.getArticle();
    this.setupViewer();
  }

  ngOnDestroy() {
    this._subscription.unsubscribe();
  }

  private async getArticle() {
    this._subscription.add(
      this._route.data.subscribe(async (data: { article: ArticleModel }) => {
        this.model = data.article;
        this._titleService.setTitle(
          `${this.model.title} - ${constants.titlePlainText}`
        );

        await timeout(10);
      })
    );
  }

  private async setupViewer() {
    this.content = this.model.content;
    if (!this.store.renderFromClient) return;

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
