import { Injectable } from "@angular/core";
import { BaseService, ISubService } from "src/app/shared/services/base.service";
import { ArticleParameters } from "../models/article-parameters";
import { PagedResult } from "src/app/shared/models/response-result";
import ArticleModel from "../models/article-model";

@Injectable({
  providedIn: "root"
})
export class ArticleService extends BaseService implements ISubService {
  serviceName = "article";
  /**
   * 获取分页文章
   * @param postParameter any 或 ArticleParameters
   */
  async getPagedArticles(
    params?: ArticleParameters
  ): Promise<PagedResult<ArticleModel>> {
    var res = await this.get(this.serviceName, params);
    return res.succeed && (res.data as PagedResult<ArticleModel>);
  }

  /**
   * 获取文章
   */
  async getArticle(id: number): Promise<ArticleModel> {
    var res = await this.get(`${this.serviceName}/${id}`);
    return res.succeed && (res.data as ArticleModel);
  }

  /**
   * 添加文章
   * @param params
   */
  async add(params?: ArticleModel): Promise<ArticleModel> {
    var res = await this.post(this.serviceName, params);
    return res.succeed && (res.data as ArticleModel);
  }
}
