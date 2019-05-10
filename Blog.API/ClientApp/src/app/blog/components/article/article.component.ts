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
  ngOnInit() {
    this.getArticle();
  }

  ngOnDestroy() {
    this._subscription.unsubscribe();
  }

  private getArticle() {
    this._subscription.add(
      this.store.routeDataChanged$.subscribe(x => {
        if (!x.article) return;
        this.model = x.article;
        this._titleService.setTitle(
          `${this.model.title} - ${constants.titlePlainText}`
        );
        this.setupViewer();
      })
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
