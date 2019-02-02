import { Component, OnInit, ViewChild } from '@angular/core';
import { MarkdownService } from 'src/app/shared/services/markdown.service';
import { ArticleService } from '../../services/article.service';
import { ArticleParameters } from '../../models/article-parameters';
import { TdTextEditorComponent } from '@covalent/text-editor';

@Component({
  selector: 'app-write-article',
  templateUrl: './write-article.component.html',
  styleUrls: ['./write-article.component.scss']
})
export class WriteArticleComponent implements OnInit {
  @ViewChild('editor') private editor: TdTextEditorComponent;
  parameters = new ArticleParameters({ title: "啊哈哈哈哈哈", content: "" })

  constructor(private mdService: MarkdownService, private service: ArticleService) {
  }
  ngOnInit() {
  }
  async submit() {
    // TODO: 这个好像做不了双向绑定
    this.parameters.content = this.editor.value
    var res = this.service.add(this.parameters)
  }
}
