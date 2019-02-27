import { Component, OnInit } from "@angular/core";
import ArticleModel from "../../models/article-model";

@Component({
  selector: "app-home",
  template: `
    <app-header [model]="headerModel"></app-header>
    <app-article-list></app-article-list>
  `
})
export class HomeComponent implements OnInit {
  headerModel: ArticleModel = new ArticleModel();

  constructor() {
    console.log("home ctor.");
    this.headerModel.title = "siegrain🌌wang";
  }

  ngOnInit() {}
}
