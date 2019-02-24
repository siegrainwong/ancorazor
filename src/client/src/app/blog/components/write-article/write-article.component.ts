import { Component, OnInit } from "@angular/core";
import { ArticleService } from "../../services/article.service";
import { Router } from "@angular/router";
import ArticleModel from "../../models/article-model";
import { Variables } from "src/app/shared/variables";

@Component({
  selector: "app-write-article",
  templateUrl: "./write-article.component.html",
  styleUrls: ["./write-article.component.scss"]
})
export class WriteArticleComponent implements OnInit {
  model = new ArticleModel();

  constructor(
    private service: ArticleService,
    private router: Router,
    variables: Variables
  ) {}
  ngOnInit() {}
  async submit() {
    var res = await this.service.add(this.model);
    var model = res.data as ArticleModel;
    this.router.navigate([`[./${model.id}]`]);
  }
}
