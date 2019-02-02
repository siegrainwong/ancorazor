import { Component, OnInit, ViewChild } from '@angular/core';
import { MarkdownService } from 'src/app/shared/services/markdown.service';
import { ArticleService } from '../../services/article.service';
import { ArticleParameters } from '../../models/article-parameters';
import { TdTextEditorComponent } from '@covalent/text-editor';
import { Router } from '@angular/router';
import ArticleModel from '../../models/article-model';

@Component({
  selector: 'app-write-article',
  templateUrl: './write-article.component.html',
  styleUrls: ['./write-article.component.scss']
})
export class WriteArticleComponent implements OnInit {
  @ViewChild('editor') private editor: TdTextEditorComponent;
  parameters = new ArticleParameters({ title: "啊哈哈哈哈哈", content: "" })

  constructor(private mdService: MarkdownService,
    private service: ArticleService,
    private router: Router) {
  }
  ngOnInit() {
  }
  async submit() {
    // TODO: 这个好像做不了双向绑定
    this.parameters.content = this.editor.value
    var res = await this.service.add(this.parameters)
    var model = res.data as ArticleModel;
    // TODO: 等做好详情页后看下这里怎么跳
    this.router.navigate([`[./${model.id}]`])
  }
}
