import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { BlogRoutingModule } from "./blog.routing";
import { BlogAppComponent } from "./blog-app.component";
import { NavComponent } from "./components/nav/nav.component";
import { ArticleService } from "./services/article.service";
import { ArticleListComponent } from "./components/article-list/article-list.component";
import { WriteArticleComponent } from "./components/write-article/write-article.component";
import { HeaderComponent } from "./components/header/header.component";
import { FooterComponent } from "./components/footer/footer.component";
import { AboutComponent } from "./components/about/about.component";
import { ArticleComponent } from "./components/article/article.component";
import { MaterialModule } from "../shared/material/material.module";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { SGUtil } from "../shared/utils/siegrain.utils";
import { LoggingService } from "../shared/services/logging.service";
import { SignInComponent } from "./components/sign-in/sign-in.component";
import { UserService } from "./services/user.service";
import { ConfirmDialog } from "../shared/components/confirm-dialog.component";
import { AuthGuard } from "../shared/guard/auth.guard";
import { ScrollDispatchModule } from "@angular/cdk/scrolling";
import { ArticleResolveGuard } from "./guard/article.resolve.guard";
import {
  SGTransitionResolveGuard,
  TestObservableResolveGuard,
  TestDirectResolveGuard
} from "../shared/animations/sg-transition.resolve.guard";
import { ArticleListResolveGuard } from "./guard/article-list.resolve.guard";
import { SGProgress } from "../shared/utils/siegrain.progress";
import { SEOService } from "./services/seo.service";
import { CommonService } from "./services/common.service";
import { ObservedComponentBase } from "../shared/components/observed.base";
import { PageNotFoundComponent } from "./components/page-not-found/page-not-found.component";

/**
 * MARK: `providedIn: 'root'` or `providedIn: SomeModule`
 *
 * Angular 文档中提到过应将服务注册到你需要的模块上避免全部加载，但貌似对于现在的版本(Angular 6+)来说，
 * `AOT`编译后`tree shaking`会自动帮你把服务绑定到对应的`lazy module`上，
 * 意味着你不需要再去管这方面的事情，直接`providedIn: root`即可。
 * 就算你想绑到某个`Module`上，也有极高概率会遇到TS的循环引用提示。
 *
 * ref: https://github.com/angular/angular-cli/issues/10170#issuecomment-380673276
 */
@NgModule({
  declarations: [
    ObservedComponentBase,
    BlogAppComponent,
    NavComponent,
    ArticleListComponent,
    WriteArticleComponent,
    HeaderComponent,
    FooterComponent,
    AboutComponent,
    ArticleComponent,
    SignInComponent,
    PageNotFoundComponent,
    ConfirmDialog
  ],
  entryComponents: [SignInComponent, ConfirmDialog],
  imports: [
    CommonModule,
    BlogRoutingModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    ScrollDispatchModule
  ],
  providers: [
    SGUtil,
    SGProgress,
    CommonService,
    ArticleService,
    UserService,
    SEOService,
    AuthGuard,
    ArticleResolveGuard,
    SGTransitionResolveGuard,
    ArticleListResolveGuard
  ]
})
export class BlogModule {
  constructor(logger: LoggingService) {
    logger.info("blog ctor.");
  }
}
