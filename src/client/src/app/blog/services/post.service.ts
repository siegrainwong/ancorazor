import { Injectable } from '@angular/core';
import { BaseService } from 'src/app/shared/base.service';
import { ArticleParameters } from '../models/article-parameters';
import { Result } from 'src/app/shared/models/response-result';
import ArticleModel from '../models/article-model';

@Injectable({
  providedIn: 'root'
})
export class PostService extends BaseService {
  /**
   * Mark: 多类型声明
   * @param postParameter any 或 ArticleParameters
   */
  async getPagedPosts(params?: any | ArticleParameters): Promise<Result<ArticleModel>> {
    var res = await this.get("articles", params)
    return res.castTo<ArticleModel>()
  }
}