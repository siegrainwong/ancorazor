import BaseModel from "src/app/shared/models/base-model";

export default class ArticleModel extends BaseModel {
  author: string;
  title: string;
  digest: string;
  alias: string;
  tags?: { name: string; alias: string }[];
  category?: { name: string; alias: string };
  content: string;
  viewCount: number = 0;
  commentCount: number = 0;
  path: string;
  cover?: string | number;
  isDraft: boolean = false;
  previous: { title: string; path: string };
  next: { title: string; path: string };

  constructor(init?: Partial<ArticleModel>) {
    super(init);
    Object.assign(this, init);
  }
}
