import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-write-article',
  templateUrl: './write-article.component.html',
  styleUrls: ['./write-article.component.scss']
})
export class WriteArticleComponent implements OnInit {
  options: any = {
    lineWrapping: true,
    toolbar: false,
  }
  someText: string = "asdfasdfasdfasdf"

  constructor() { }
  ngOnInit() {
  }
}
