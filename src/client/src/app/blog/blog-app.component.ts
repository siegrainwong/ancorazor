/**
 * Mark: ng-cli 创建单ts页面
 * ng g c blog/blog-app --flat --module blog --inline-style --inline-template
 * --flat：前缀式生成
 * --module blog：指定声明模块
 * --inline-style --inline-template：不分开生成scss和html文件，只生成一个ts
 */

import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-blog-app',
  template: `<app-nav></app-nav>`,
  styles: []
})
export class BlogAppComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

}
