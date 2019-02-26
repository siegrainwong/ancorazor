import { NgModule, Compiler, Injector } from "@angular/core";
import { CommonModule } from "@angular/common";
import { BlogRoutingModule } from "./blog-routing.module";
import { MaterialModule } from "../shared/material/material.module";
import { BlogAppComponent } from "./blog-app.component";
import { NavComponent } from "./components/nav/nav.component";
import { ArticleService } from "./services/article.service";
import { ArticleListComponent } from "./components/article-list/article-list.component";
import { ArticleItemComponent } from "./components/article-item/article-item.component";
import { WriteArticleComponent } from "./components/write-article/write-article.component";
import { HeaderComponent } from "./components/header/header.component";
import { FooterComponent } from "./components/footer/footer.component";
import { HomeComponent } from "./components/home/home.component";
import { AboutComponent } from "./components/about/about.component";
import { ArticleComponent } from "./components/article/article.component";
import { FormsModule } from "@angular/forms";
import { Variables } from "../shared/variables";
import { CodemirrorModule } from "@ctrl/ngx-codemirror";

let imports = [
  CommonModule,
  BlogRoutingModule,
  MaterialModule,
  FormsModule,
  CodemirrorModule
];

console.log("process.env.renderFromServer:", process.env.renderFromServer);

// if (window) {
//   console.log("import extra modules");

//   let CovalentTextEditorModule = require("@covalent/text-editor")
//     .CovalentTextEditorModule;
//   imports.push(CovalentTextEditorModule);
// }

@NgModule({
  declarations: [
    BlogAppComponent,
    NavComponent,
    ArticleListComponent,
    ArticleItemComponent,
    WriteArticleComponent,
    HeaderComponent,
    FooterComponent,
    HomeComponent,
    AboutComponent,
    ArticleComponent
  ],
  imports: [
    CommonModule,
    BlogRoutingModule,
    MaterialModule,
    FormsModule
  ],
  providers: [ArticleService]
})
export class BlogModule {
  constructor(
    private variables: Variables,
    private compiler: Compiler,
    private injector: Injector
  ) {
    console.log("blog ctor.");
  }

  // async ngOnInit() {
  //   if (this.variables.renderFromServer) return;

  //   SystemJS.set("@angular/core", SystemJS.newModule(AngularCore));
  //   SystemJS.set("@angular/common", SystemJS.newModule(AngularCommon));

  //   // now, import the new module
  //   let module = await SystemJS.import("@covalent/text-editor/index.d.ts");
  //   console.log("module loaded:", module);
  //   this.compiler
  //     .compileModuleAndAllComponentsAsync(module.default)
  //     .then(compiled => {
  //       let moduleRef = compiled.ngModuleFactory.create(this.injector);
  //       let factory = compiled.componentFactories[0];
  //       if (factory) {
  //         console.log("module loaded:", factory);

  //         // let component = this.vc.createComponent(factory);
  //         // let instance = component.instance;
  //       }
  //     });
  // }
}
