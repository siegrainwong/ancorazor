import { QueryParameters } from "../../shared/models/query-parameters";

export class ArticleParameters extends QueryParameters {
  title?: string;

  constructor(init?: Partial<ArticleParameters>) {
    super(init);
    Object.assign(this, init);
  }
}
