import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";

const isDebugMode = !environment.production;
// const isDebugMode = true;

const noop = (): any => undefined;

/**
 * 日志服务
 * https://robferguson.org/blog/2017/09/09/a-simple-logging-service-for-angular-4/
 */
@Injectable()
export class LoggingService {
  constructor() {}

  get info() {
    if (isDebugMode) {
      return console.info.bind(console);
    } else {
      return noop;
    }
  }

  get warn() {
    if (isDebugMode) {
      return console.warn.bind(console);
    } else {
      return noop;
    }
  }

  get error() {
    if (isDebugMode) {
      return console.error.bind(console);
    } else {
      return noop;
    }
  }

  invokeConsoleMethod(type: string, args?: any): void {
    const logFn: Function = console[type] || console.log || noop;
    logFn.apply(console, [args]);
  }
}
