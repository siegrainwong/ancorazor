import { Component, OnInit, OnDestroy } from "@angular/core";
import ArticleModel from "../../models/article-model";
import { Store } from "src/app/shared/store/store";
import { Title } from "@angular/platform-browser";
import {
  constants,
  externalScripts
} from "src/app/shared/constants/siegrain.constants";
import { SGUtil } from "src/app/shared/utils/siegrain.utils";
import { Subscription } from "rxjs";
import { SGTransitionDelegate } from "src/app/shared/animations/sg-transition.delegate";
import { SGAnimations } from "src/app/shared/animations/sg-animations";
import { take, map } from "rxjs/operators";

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
    private _titleService: Title,
    private _util: SGUtil,
    public store: Store
  ) {}
  async ngOnInit() {
    await this.getArticle();
    this.setupViewer();
  }

  ngOnDestroy() {
    this._subscription.unsubscribe();
  }

  private async getArticle() {
    let article = await this.store.routeDataChanged$
      .pipe(
        take(1),
        map(x => x.article)
      )
      .toPromise();
    this.model = article;
    this._titleService.setTitle(
      `${article.title} - ${constants.titlePlainText}`
    );
  }

  private async setupViewer() {
    const renderFromClient = this.store.renderFromClient;

    if (renderFromClient)
      await this._util.loadExternalScripts([externalScripts.highlight]);

    const md = require("markdown-it")({
      highlight: function(str, lang) {
        if (renderFromClient && lang && hljs.getLanguage(lang)) {
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

    const yamlFront = require("yaml-front-matter");
    var content = yamlFront.loadFront(this.model.content).__content;
    this.content = md.render(content);
  }
}
