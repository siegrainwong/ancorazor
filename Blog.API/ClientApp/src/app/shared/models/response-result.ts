import BaseModel from "./base-model";

export class ResponseResult {
  succeed: boolean = false;
  data?: any;
  message?: string;

  constructor(obj?: Partial<ResponseResult>) {
    Object.assign(this, obj);
  }
}

export class PagedResult<T extends BaseModel> {
  list: T[];
  total: number;
  pageSize: number;
  pageIndex: number;
  pageCount: number;
  hasNext: boolean;
  hasPrevious: boolean;

  nextPageIndex: number;
  previousPageIndex: number;
}
