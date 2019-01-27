import { Injectable } from '@angular/core';
import { BaseService } from 'src/app/shared/base.service';
import { ArticleParameters } from '../models/article-parameters';

@Injectable({
  providedIn: 'root'
})
export class PostService extends BaseService {

  /**
   * Mark: Angular DI
   * @param http 
   */
  constructor() {
    super();
  }

  /**
   * Mark: 多类型声明
   * @param postParameter any 或 ArticleParameters
   */
  async getPagedPosts(params?: any | ArticleParameters) {
    return await this.get("blogs", params)
  }
}
