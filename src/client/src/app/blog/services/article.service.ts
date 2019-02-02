import { Injectable } from '@angular/core';
import { BaseService, ISubService } from 'src/app/shared/services/base.service';
import { ArticleParameters } from '../models/article-parameters';
import { Result } from 'src/app/shared/models/response-result';
import ArticleModel from '../models/article-model';

@Injectable({
  providedIn: 'root'
})
export class ArticleService extends BaseService implements ISubService {
  serviceName = "articles";
  /**
   * 获取分页文章
   * Mark: 多类型声明
   * @param postParameter any 或 ArticleParameters
   */
  async getPagedArticles(params?: ArticleParameters): Promise<Result<ArticleModel>> {
    var res = await this.get(this.serviceName, params)
    return res.castTo<ArticleModel>()
  }

  /**
   * 添加文章
   * @param params
   */
  async add(params?: ArticleParameters): Promise<Result<ArticleModel>> {
    var res = await this.post(this.serviceName, params)
    return res.castTo<ArticleModel>()
  }
}
