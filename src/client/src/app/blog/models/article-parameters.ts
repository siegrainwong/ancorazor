import { QueryParameters } from '../../shared/query-parameters';

export class ArticleParameters extends QueryParameters {
    title?: string;

    constructor(init?: Partial<ArticleParameters>) {
        super(init);
        Object.assign(this, init);
    }
}
