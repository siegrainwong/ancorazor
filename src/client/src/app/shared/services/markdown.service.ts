import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class MarkdownService {

  constructor() { }

  getConfigs(): any {
    return {
      lineWrapping: true, // wyswyg
      toolbar: false, // 关闭工具栏
    }
  }
}
