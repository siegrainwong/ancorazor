import { Component, OnInit, Input } from '@angular/core';
import ArticleModel from '../../models/article-model';

@Component({
  selector: 'app-article-item',
  templateUrl: './article-item.component.html',
  styleUrls: ['./article-item.component.scss']
})
export class ArticleItemComponent implements OnInit {

  @Input() item: ArticleModel;

  constructor() { }

  ngOnInit() {
  }
}
