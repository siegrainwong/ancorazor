import { Component, OnInit } from '@angular/core';
import { MarkdownService } from 'src/app/shared/services/markdown.service';
import { ArticleService } from '../../services/article.service';
import { ArticleParameters } from '../../models/article-parameters';
import { MatFormFieldControl } from '@angular/material';

@Component({
  selector: 'app-write-article',
  templateUrl: './write-article.component.html',
  styleUrls: ['./write-article.component.scss']
})
export class WriteArticleComponent implements OnInit {
  content: string = "asdfasdfasdfasdf"
  parameters = new ArticleParameters({ title: "啊哈哈哈哈哈", content: "" })

  constructor(private mdService: MarkdownService, private service: ArticleService) {
  }
  ngOnInit() {
  }
  async submit() {
    var res = this.service.add(this.parameters)
  }
}
