import { Component, OnInit } from "@angular/core";

@Component({
  selector: "app-home",
  template: `
    <app-header [title]="'siegrain.wang'"></app-header>
    <app-article-list></app-article-list>
  `
})
export class HomeComponent implements OnInit {
  constructor() {}

  ngOnInit() {}
}
