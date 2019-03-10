import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { BlogRoutingModule } from "./blog-routing.module";
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
import { Store } from "../shared/store/store";
import { UserService } from "./services/user.service";

@NgModule({
  declarations: [
    BlogAppComponent,
    NavComponent,
    ArticleListComponent,
    WriteArticleComponent,
    HeaderComponent,
    FooterComponent,
    AboutComponent,
    ArticleComponent,
    SignInComponent
  ],
  entryComponents: [SignInComponent],
  imports: [
    CommonModule,
    BlogRoutingModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [SGUtil, ArticleService, UserService]
})
export class BlogModule {
  constructor(logger: LoggingService) {
    logger.info("blog ctor.");
  }
}
