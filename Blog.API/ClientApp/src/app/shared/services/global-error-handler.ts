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
