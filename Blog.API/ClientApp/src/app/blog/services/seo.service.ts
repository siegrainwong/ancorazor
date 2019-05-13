import { Injectable, OnDestroy } from "@angular/core";
import { Meta, Title } from "@angular/platform-browser";
import { Subscription, combineLatest } from "rxjs";
import { Store } from "src/app/shared/store/store";
import SiteSettingModel from "../models/site-setting.model";
import ArticleModel from "../models/article-model";

@Injectable({ providedIn: "root" })
export class SEOService implements OnDestroy {
  private _subscription = new Subscription();

  constructor(
    private _meta: Meta,
    private _store: Store,
    private _titleService: Title
  ) {
    this.setup();
  }

  ngOnDestroy() {
    this._subscription.unsubscribe();
  }

  public setup() {
    this._subscription.add(
      combineLatest(
        this._store.routeDataChanged$,
        this._store.siteSettingChanged$
      ).subscribe(([routeData, setting]) => {
        if (!setting.siteName) return;
        this.setupSiteSEO(setting);
        this.setArticleSEO(routeData.article, setting.siteName);
      })
    );
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
    article.tags.length &&
      this._meta.addTag({
        name: "keywords",
        content: article.tags.map(x => x.name).join(", ")
      });
  }
}
