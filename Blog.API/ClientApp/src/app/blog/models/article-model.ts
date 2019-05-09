import BaseModel from "src/app/shared/models/base-model";
import { SGAnimation } from "src/app/shared/animations/sg-transition.model";

export default class ArticleModel extends BaseModel {
  author: string;
  title: string;
  digest: string;
  alias: string;
  tags?: { name: string; alias: string } | string[];
  categories?: { name: string; alias: string } | string[];
  content: string;
  viewCount: number = 0;
  commentCount: number = 0;
  path: string;
  cover: string = "assets/img/placeholder.jpg";
  isDraft: boolean = false;
  previous: { title: string; path: string };
  next: { title: string; path: string };

  // helper fields
  animation?: SGAnimation;

  constructor(init?: Partial<ArticleModel>) {
    super(init);
    Object.assign(this, init);
  }
}
