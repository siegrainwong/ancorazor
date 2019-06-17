import { ErrorHandler, Injectable, Injector } from "@angular/core";
import { LocationStrategy, PathLocationStrategy } from "@angular/common";
import * as StackTrace from "stacktrace-js";
import { LoggingService } from "./logging.service";

/**
 * 全局错误处理
 * https://medium.com/@amcdnl/global-error-handling-with-angular2-6b992bdfb59c
 */
@Injectable()
export class GlobalErrorHandler implements ErrorHandler {
  constructor(private _injector: Injector) {}
  handleError(error: Error) {
    const loggingService = this._injector.get(LoggingService);
    const location = this._injector.get(LocationStrategy);
    const message = error.message ? error.message : error.toString();
    const url = location instanceof PathLocationStrategy ? location.path() : "";
    /**
     * TODO: 想一下怎么做一个简单的APM
     * 最好都用elk技术栈配合skywalking前后一条龙
     * 1. 记录首屏加载，文档加载(DOMContentLoaded)，页面加载(load)，脚本错误，慢路由
     * 2. 跟踪业务流程
     * 3. 离线存储和在线存储
     * 4. 生成Source-map，webpack打包后的代码报错你是trace不到的
     * 5. 错误预警，统计报表
     */
    // get the stack trace, lets grab the last 10 stacks only
    StackTrace.fromError(error).then(stackframes => {
      const stackString = stackframes
        .splice(0, 20)
        .map(function(sf) {
          return sf.toString();
        })
        .join("\n");
      // log on the server
      loggingService.error({ message, url, stack: stackString });
    });
    throw error;
  }
}
