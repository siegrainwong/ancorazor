import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";
import { AxiosResponse, AxiosRequestConfig } from "axios";
import ResponseResult from "../models/response-result";
import { OpenIdConnectService } from "../oidc/open-id-connect.service";
import axios from "axios";

@Injectable({
  providedIn: "root"
})
export abstract class BaseService {
  constructor(private userService: OpenIdConnectService) {
    this.setup();
  }

  setup() {
    axios.defaults.baseURL = environment.apiUrlBase;
    axios.defaults.timeout = 20000;
    axios.defaults.headers = { "Content-Type": "application/json" };
    axios.interceptors.request.use(
      config => {
        if (this.userService.userIsAvailable)
          config.headers.Authorization = `${this.userService.user.token_type} ${
            this.userService.user.access_token
          }`;
        return config;
      },
      err => {
        return Promise.reject(err);
      }
    );
  }

  async get(
    url: string,
    query?: any,
    option?: AxiosRequestConfig
  ): Promise<ResponseResult> {
    return await this.handleRequest(Methods.GET, url, null, query, option);
  }
  async post(
    url: string,
    body: any,
    query?: any,
    option?: AxiosRequestConfig
  ): Promise<ResponseResult> {
    return await this.handleRequest(Methods.POST, url, body, query, option);
  }
  async put(
    url: string,
    body?: any,
    query?: any,
    option?: AxiosRequestConfig
  ): Promise<ResponseResult> {
    return await this.handleRequest(Methods.PUT, url, body, query, option);
  }
  async delete(
    url: string,
    query?: any,
    option?: AxiosRequestConfig
  ): Promise<ResponseResult> {
    return await this.handleRequest(Methods.DELETE, url, null, query, option);
  }

  /**
   * 请求处理
   * @param method Method
   * @param url URL
   * @param body Payload
   * @param query QueryString
   * @param option 选项（未实装）
   */
  async handleRequest(
    method: Methods,
    url: string,
    body?: any,
    query?: any,
    option?: AxiosRequestConfig
  ): Promise<ResponseResult> {
    try {
      const res = await axios.request({
        method: method.toString(),
        url: url,
        params: query,
        data: body,
        withCredentials: false
      });
      return this.handleResponse(res);
    } catch (error) {
      if (error.response) {
        return this.handleResponse(error.response as AxiosResponse);
      } else {
        console.log(error);
        return null;
      }
    }
  }

  /**
   * 响应处理
   * @param response
   */
  handleResponse(response: AxiosResponse): ResponseResult {
    let result: ResponseResult = null;
    if (response.status >= 200 && response.status < 300)
      result = new ResponseResult(
        response.data.data,
        response.data.succeed,
        response.data.pagination
      );
    else
      return this.handleError(
        new ResponseResult(
          `Request failed : status ${response.status} ${response.statusText}`
        )
      );

    if (result.succeed) {
      return result;
    } else {
      return this.handleError(new ResponseResult(result));
    }
  }

  /**
   * 错误处理
   * @param result
   */
  handleError(result: ResponseResult): ResponseResult {
    console.log(result.data);
    return new ResponseResult(result);
  }
}

export interface ISubService {
  /** 服务名称 */
  serviceName: string;
}

const enum Methods {
  GET = "GET",
  POST = "POST",
  PUT = "PUT",
  DELETE = "DELETE"
}
