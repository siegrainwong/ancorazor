import { Injectable } from '@angular/core';
import { BaseService } from 'src/app/shared/base.service';
import { ArticleParameters } from '../models/article-parameters';
import ResponseModel from 'src/app/shared/models/response-model';
import ArticleModel from '../models/article-model';

@Injectable({
  providedIn: 'root'
})
export class PostService extends BaseService {
  /**
   * Mark: 多类型声明
   * @param postParameter any 或 ArticleParameters
   */
  async getPagedPosts(params?: any | ArticleParameters): Promise<ResponseModel<ArticleModel>> {
    var res = await this.get("articles", params)
    res.data = res.data as ArticleModel[]
    return res
  }
}
