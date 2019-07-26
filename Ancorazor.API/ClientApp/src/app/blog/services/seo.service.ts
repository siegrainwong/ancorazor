import { Injectable, OnDestroy } from "@angular/core";
import { Meta, Title } from "@angular/platform-browser";
import { combineLatest } from "rxjs";
import { Store } from "src/app/shared/store/store";
import SiteSettingModel from "../models/site-setting.model";
import ArticleModel from "../models/article-model";
import { ObservedServiceBase } from "src/app/shared/components/observed.base";
import { AutoUnsubscribe } from "src/app/shared/utils/auto-unsubscribe.decorator";

@Injectable({ providedIn: "root" })
@AutoUnsubscribe()
export class SEOService extends ObservedServiceBase implements OnDestroy {
  private _routeAndSettingChanged$;
  constructor(
    private _meta: Meta,
    private _store: Store,
    private _titleService: Title
  ) {
    super();
    this.setup();
  }

  public setup() {
    this._routeAndSettingChanged$ = combineLatest(
      this._store.routeDataChanged$,
      this._store.siteSettingChanged$
    ).subscribe(([routeData, setting]) => {
      if (!setting || !setting.siteName) return;
      this.setupSiteSEO(setting);
      this.setArticleSEO(routeData.article, setting.siteName);
    });
  }

  private setupSiteSEO(setting: SiteSettingModel) {
    this._titleService.setTitle(setting.siteName);
    this._meta.addTag({ property: "og:site_name", content: setting.siteName });
    setting.keywords &&
      this._meta.addTag({ name: "keywords", content: setting.keywords });
  }

  private setArticleSEO(article: ArticleModel, siteName: string) {
    if (!article) return;
    this._titleService.setTitle(`${article.title} - ${siteName}`);
    this._meta.addTag({
      name: "title",
      content: article.title
    });
    this._meta.addTag({ name: "author", content: article.author });
    this._meta.addTag({
      name: "date",
      content: article.createdAt.toString()
    });
    article.digest &&
      this._meta.addTag({
        name: "description",
        content: article.digest
      });
    article.tags &&
      article.tags.length &&
      this._meta.addTag({
        name: "keywords",
        content: article.tags.map(x => x.name).join(", ")
      });
  }
}
