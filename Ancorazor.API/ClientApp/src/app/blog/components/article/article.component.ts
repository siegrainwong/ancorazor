import { Component, OnInit, OnDestroy } from "@angular/core";
import ArticleModel from "../../models/article-model";
import { Store } from "src/app/shared/store/store";
import { externalScripts } from "src/app/shared/constants/siegrain.constants";
import { SGUtil, topElementId } from "src/app/shared/utils/siegrain.utils";
import { SGTransitionDelegate } from "src/app/shared/animations/sg-transition.delegate";
import { SGAnimations } from "src/app/shared/animations/sg-animations";
import { ObservedComponentBase } from "src/app/shared/components/observed.base";
import { AutoUnsubscribe } from "src/app/shared/utils/auto-unsubscribe.decorator";
import { SGRouteTransitionCommands } from "src/app/shared/animations/sg-transition.model";
import { ActivatedRouteSnapshot } from "@angular/router";
import { timeout } from "src/app/shared/utils/promise-delay";

@Component({
  selector: "app-article",
  templateUrl: "./article.component.html",
  styleUrls: ["./article.component.scss"]
})
@AutoUnsubscribe()
export class ArticleComponent extends ObservedComponentBase
  implements OnInit, OnDestroy, SGTransitionDelegate {
  public animations = {
    content: SGAnimations.fade
  };
  public model: ArticleModel;
  public content: string;

  private _routeChanged$;
  constructor(private _util: SGUtil, public store: Store) {
    super();
  }
  ngOnInit() {
    this.getArticle();
  }

  transitionForComponent?(
    nextRoute: ActivatedRouteSnapshot
  ): SGRouteTransitionCommands {
    return new SGRouteTransitionCommands({ scrollTo: topElementId });
  }
  private getArticle() {
    this._routeChanged$ = this.store.routeDataChanged$.subscribe(x => {
      if (!x.article) return;
      this.model = x.article;
      this.setupViewer();
      this.setupComment();
    });
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

  private async setupComment() {
    if (!this.store.renderFromClient) return;
    await this._util.loadExternalScripts([externalScripts.gitment]);

    const setting = this.store.siteSetting && this.store.siteSetting.gitment;
    if (!setting) return;
    await timeout(10); // waiting for render
    new Gitment.construct({
      id: this.model.id.toString(),
      owner: setting.githubId,
      repo: setting.repositoryName,
      oauth: {
        client_id: setting.clientId,
        client_secret: setting.clientSecret
      }
    }).render(document.getElementById("comments"));
  }
}
