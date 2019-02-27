export abstract class QueryParameters {
  pageIndex: number = 0;
  pageSize: number = 10;
  fields?: string;
  orderBy: string = "id desc";

  constructor(init?: Partial<QueryParameters>) {
    Object.assign(this, init);
  }
}
