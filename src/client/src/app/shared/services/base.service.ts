import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";
import { AxiosResponse, AxiosRequestConfig } from "axios";
import { ResponseResult } from "../models/response-result";
// import { OpenIdConnectService } from "../oidc/open-id-connect.service";
import axios from "axios";
import { LoggingService } from "./logging.service";
import { SGUtil } from "../utils/siegrain.utils";
import { Store } from "../store/store";

@Injectable({
  providedIn: "root"
})
export abstract class BaseService {
  constructor(
    private logger: LoggingService,
    private util: SGUtil,
    public store: Store
  ) {
    this.setup();
  }

  setup() {
    axios.defaults.baseURL = environment.apiUrlBase;
    axios.defaults.timeout = 100000;
    axios.defaults.headers = { "Content-Type": "application/json" };
    axios.interceptors.request.use(
      config => {
        if (this.store.user)
          config.headers.Authorization = `Bearer ${this.store.user.token}`;
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
        this.util.tip(error);
        this.logger.error(error);
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
      result = new ResponseResult({
        data: response.data.data,
        succeed: response.data.succeed,
        message: response.data.message
      });
    else
      return this.handleError(
        new ResponseResult({
          message: `Request failed : status ${response.status} ${
            response.statusText
          }`
        }),
        response.status
      );

    if (result.succeed) return result;
    else return this.handleError(new ResponseResult(result));
  }

  /**
   * 错误处理
   * @param result
   */
  handleError(result: ResponseResult, code?: number): ResponseResult {
    switch (code) {
      case 403:
        this.util.tip("认证过期，请重新登录！");
        this.store.user = null;
        break;
      default:
        this.util.tip(result.message);
        this.logger.error(result);
    }
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
