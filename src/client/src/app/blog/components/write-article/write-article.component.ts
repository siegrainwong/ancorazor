import { Component, OnInit, ViewChild } from '@angular/core';
import { TdTextEditorComponent } from '@covalent/text-editor';

@Component({
  selector: 'app-write-article',
  templateUrl: './write-article.component.html',
  styleUrls: ['./write-article.component.scss']
})
export class WriteArticleComponent implements OnInit {
  @ViewChild('textEditor') private _textEditor: TdTextEditorComponent;

  options: any = {
    lineWrapping: true,
    toolbar: false,
  };

  constructor() { }
  ngOnInit() {
  }
  ngAfterViewInit(): void {
    this._textEditor.togglePreview();
  }
}
