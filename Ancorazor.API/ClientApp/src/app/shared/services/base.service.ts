import { Injectable, OnDestroy } from "@angular/core";
import { environment } from "src/environments/environment";
import { AxiosResponse } from "axios";
import { ResponseResult } from "../models/response-result";
import axios from "axios";
import { LoggingService } from "./logging.service";
import { SGUtil } from "../utils/siegrain.utils";
import { Store } from "../store/store";
import { TaskWrapper } from "./async-helper.service";
import { TransferState, makeStateKey } from "@angular/platform-browser";
import { Router } from "@angular/router";
import { Subscription } from "rxjs";
import { SGProgress, SGProgressMode } from "../utils/siegrain.progress";
import { setupCache } from "axios-cache-adapter";

@Injectable({ providedIn: "root" })
export abstract class BaseService implements ISGService {
  serviceName = null;
  protected settings = { disabledCache: false };
  private _api = axios.create({
    baseURL: environment.apiUrlBase,
    timeout: 100000,
    headers: { "Content-Type": "application/json" },
    adapter: this.cacheAdapter
  });
  private _url: string;

  constructor(
    private _wrapper: TaskWrapper,
    private _state: TransferState,
    private _progress: SGProgress,
    protected util: SGUtil,
    protected route: Router,
    protected logger: LoggingService,
    protected store: Store
  ) {
    this.initialize();
  }

  /**
   * initialize for child services
   */
  protected abstract initialize();
  /**
   * Set the service should be exclude from cache
   */
  protected disabledCache = () => false;

  public async get(url: string, query?: any): Promise<ResponseResult> {
    /**
     * Mark: 让`Server side`的请求结果传递到`Client side`避免重复请求
     * https://medium.com/@evertonrobertoauler/angular-5-universal-with-transfer-state-using-angular-cli-19fe1e1d352c
     */
    const stateKey = makeStateKey(url);
    const storedData = this._state.get(stateKey, null);
    if (storedData) {
      this._state.remove(stateKey);
      const response = storedData as ResponseResult;
      if (response.succeed) {
        this.logger.info("server side state fetched: ", storedData);
        return Promise.resolve(storedData as ResponseResult);
      } else {
        this.logger.error("server side request failed: ", storedData);
      }
    }

    /**
     * Mark: 让`universal`等待`API`请求并渲染完毕
     * https://github.com/angular/angular/issues/20520#issuecomment-449597926
     */
    return new Promise<ResponseResult>(resolve => {
      if (!this.store.renderFromClient)
        this.logger.info(
          "Server side requesting",
          `${environment.apiUrlBase}/${url}`
        );
      this._wrapper
        .doTask(this.handleRequest(Methods.GET, url, null, query))
        .subscribe(result => {
          if (!this.store.renderFromClient) {
            this._state.set(stateKey, result);
            this.logger.info("transfer state stored: ", result);
          }
          resolve(result);
        });
    });
  }

  public async post(
    url: string,
    body: any,
    query?: any
  ): Promise<ResponseResult> {
    return await this.handleRequest(Methods.POST, url, body, query);
  }

  public async put(
    url: string,
    body?: any,
    query?: any
  ): Promise<ResponseResult> {
    return await this.handleRequest(Methods.PUT, url, body, query);
  }

  public async delete(url: string, query?: any): Promise<ResponseResult> {
    return await this.handleRequest(Methods.DELETE, url, null, query);
  }

  /**
   * 请求前调用
   */
  protected beforeRequest(url: string) {
    this._url = url;
    this.store.isRequesting = true;
    this._progress.progressStart();
  }

  /**
   * 响应后调用
   *
   * 写这个其实是因为`axios`的`interceptor`有bug，有时候响应太快拦不到`response`
   */
  protected afterResponse(response?: AxiosResponse) {
    response &&
      response.config.method === "get" &&
      this.logger.info(
        `${this._url} responsed from cache`,
        !!response.request.fromCache
      );
    this.store.isRequesting = false;
    this._progress.progressDone(SGProgressMode.manually);
  }

  /**
   * 请求处理
   * @param method Method
   * @param url URL
   * @param body Payload
   * @param query QueryString
   * @param option 选项（未实装）
   */
  protected async handleRequest(
    method: Methods,
    url: string,
    body?: any,
    query?: any
  ): Promise<ResponseResult> {
    this.beforeRequest(url);

    try {
      const res = await this._api.request({
        method: method,
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
        this.afterResponse();
        this.util.tip(error);
        this.logger.error(error);
        return new ResponseResult({
          message: `${error.message} for url: ${error.request.url}`
        });
      }
    }
  }

  /**
   * 响应处理
   * @param response
   */
  protected handleResponse(response: AxiosResponse): ResponseResult {
    this.afterResponse(response);

    let result: ResponseResult = null;
    if (response.status >= 200 && response.status < 400)
      result = new ResponseResult({
        data: response.data.data,
        succeed: response.data.succeed,
        message: response.data.message
      });
    else {
      return this.handleError(
        new ResponseResult({
          message: `Request failed : ${response.status} ${response.statusText}`,
          data: response.data
        }),
        response.status
      );
    }

    if (result.succeed) return result;
    else return this.handleError(new ResponseResult(result));
  }

  /**
   * 错误处理
   * @param result
   */
  protected handleError(result: ResponseResult, code?: number): ResponseResult {
    switch (code) {
      case 401:
        this.util.tip("Session expired, please sign-in again.");
        this.store.signOut();
        this.route.navigate([], { fragment: "sign-in" });
        break;
      default:
        if (result.data.message) result.message += "：" + result.data.message;
        this.util.tip(result.message);
        this.logger.error(result);
    }
    return new ResponseResult(result);
  }

  private get cacheAdapter() {
    return setupCache({
      maxAge: 60 * 60 * 1000, // 1 hour
      readOnError: (error, request) => {
        if (error.response) {
          return error.response.status >= 400 && error.response.status < 600;
        } else {
          this.util.tip(error.message);
          return false;
        }
      },
      clearOnStale: false,
      exclude: {
        // disable cache from requests with query params
        query: false,
        filter: () => this.disabledCache()
      },
      // invalidate caches with same prefix on url when sending a PUT | DELETE | POST request
      invalidate: async (cfg, req) => {
        if (req.method === "get" || this.disabledCache() || !this.serviceName)
          return;
        const shouldInvalidateCachePrefix = `${environment.apiUrlBase}/${
          this.serviceName
        }`;
        if (req.url.startsWith(shouldInvalidateCachePrefix)) {
          Object.keys(cfg.store.store)
            .filter(x => x.startsWith(shouldInvalidateCachePrefix))
            .forEach(async x => await cfg.store.removeItem(x));
          this.logger.info(
            `Caches with prefix ${shouldInvalidateCachePrefix} has been removed.`
          );
        }
      }
    }).adapter;
  }

  protected deserializeJsonFromBackend(json) {
    if (typeof json !== "string") return json;
    return this.util.toCamel(JSON.parse(unescape(json)));
  }
}

export interface ISGService {
  /** 服务名称 */
  serviceName: string;
}

const enum Methods {
  GET = "GET",
  POST = "POST",
  PUT = "PUT",
  DELETE = "DELETE"
}
