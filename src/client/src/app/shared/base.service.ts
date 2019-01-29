import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { AxiosResponse, AxiosRequestConfig } from 'axios';
import ResponseModel, { Pagination } from '../shared/models/response-model'
import axios from 'axios';

axios.defaults.baseURL = environment.apiUrlBase;
axios.defaults.timeout = 20000;
axios.defaults.headers = {
  'Content-Type': 'application/x-www-form-urlencoded',
};

@Injectable({
  providedIn: 'root'
})
export abstract class BaseService {

  constructor() { }

  async get(url: string, query?: any, option?: AxiosRequestConfig) {
    return await this.handleRequest(Methods.GET, url, null, query, option);
  }
  async post(url: string, body: any, query?: any, option?: AxiosRequestConfig) {
    return await this.handleRequest(Methods.POST, url, body, query, option);
  }
  async put(url: string, body?: any, query?: any, option?: AxiosRequestConfig) {
    return await this.handleRequest(Methods.PUT, url, body, query, option);
  }
  async delete(url: string, query?: any, option?: AxiosRequestConfig) {
    return await this.handleRequest(Methods.DELETE, url, null, query, option);
  }

  /**
   * 请求处理
   * @param method Method
   * @param url URL
   * @param body Payload
   * @param query QueryString
   * @param option 选项
   */
  async handleRequest(method: Methods, url: string, body?: any, query?: any, option?: AxiosRequestConfig) {
    try {
      const res = await axios.request({
        method: method.toString(),
        url: url,
        params: query,
        data: body,
        withCredentials: false
      })
      return this.handleResponse(res)
    } catch (error) {
      if (error.response) {
        console.log(error.response)
        return this.handleResponse(error.response as AxiosResponse)
      } else {
        console.log(error)
      }
    }
  }

  /**
   * 响应处理
   * @param response 
   */
  handleResponse(response: AxiosResponse): ResponseModel<any> {
    let result = null;
    switch (response.status) {
      case 200:
        result = new ResponseModel(response.data.data, response.data.succeed)
        break;
      default:
        return new ResponseModel(`Request failed : status ${response.status} ${response.statusText}`)
    }

    if (result.succeed) {
      return new ResponseModel(result.data, true, response.data.pagination)
    } else {
      console.log(result.data);
      return new ResponseModel(result)
    }
  }
}

const enum Methods {
  GET = 'GET',
  POST = 'POST',
  PUT = 'PUT',
  DELETE = 'DELETE'
}