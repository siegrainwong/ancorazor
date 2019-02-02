import BaseModel from './base-model';
import * as _ from "lodash";

export default class ResponseResult implements IResponse {
  succeed: boolean = false
  data?: any
  pagination?: any

  constructor(data?: any, succeed: boolean = false, pagination?: any) {
    this.data = this.toCamel(data)
    this.succeed = succeed
    if (pagination) this.pagination = this.toCamel(pagination)
  }

  public castTo<T extends BaseModel>(): Result<T> {
    return new Result(this.data as T[], this.succeed, this.pagination && this.pagination as Pagination);
  }

  /**
   * Cast pascalcase to camelcase uses loadash
   * @param obj
   */
  private toCamel(obj: any) {
    if (!_.isObject(obj)) {
      return obj
    } else if (_.isArray(obj)) {
      return obj.map((v) => this.toCamel(v))
    }
    return _.reduce(obj, (r, v, k) => {
      return {
        ...r, [_.camelCase(k)]: this.toCamel(v)
      }
    }, {})
  }
}

export class Result<T extends BaseModel> implements Omit<IResponse, 'data' | 'pagination'> {
  succeed: boolean
  data: T | T[] | string
  pagination?: Pagination

  constructor(data: T[], succeed: boolean, pagination?: Pagination) {
    this.data = data
    this.succeed = succeed
    this.pagination = pagination
  }
}

export class Pagination {
  totalItemsCount: number
  pageSize: number
  pageIndex: number
  pageCount: number
  previousPageLink: string
  nextPageLink: string
}

interface IResponse {
  succeed: boolean
  data?: any
  pagination?: any
}
