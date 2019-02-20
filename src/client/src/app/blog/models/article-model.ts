import BaseModel from "src/app/shared/models/base-model";

export default class ArticleModel extends BaseModel {
  author: string;
  title: string;
  digest: string;
  category: string;
  content: string;
  viewCount: number;
  commentCount: number;
}
