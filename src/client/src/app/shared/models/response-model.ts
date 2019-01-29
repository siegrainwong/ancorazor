import BaseModel from './base-model';

export default class ResponseModel<T extends BaseModel> {
    succeed: boolean = false
    data?: any | T[]
    pagination?: Pagination

    constructor(data?: any, succeed: boolean = false, pagination?: any) {
        this.data = data;
        this.succeed = succeed;
        if (pagination) this.pagination = pagination as Pagination
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