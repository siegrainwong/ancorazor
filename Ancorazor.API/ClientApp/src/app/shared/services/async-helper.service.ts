import { Observable, Observer, Subscription } from "rxjs";

declare const Zone: any;
abstract class ZoneMacroTaskWrapper<S, R> {
  wrap(request: S): Observable<R> {
    return new Observable((observer: Observer<R>) => {
      let task;
      let scheduled = false;
      let sub: Subscription | null = null;
      let savedResult: any = null;
      let savedError: any = null;

      const scheduleTask = (_task: any) => {
        task = _task;
        scheduled = true;

        const delegate = this.delegate(request);
        sub = delegate.subscribe(
          res => (savedResult = res),
          err => {
            if (!scheduled) {
              throw new Error(
                "An http observable was completed twice. This shouldn't happen, please file a bug."
              );
            }
            savedError = err;
            scheduled = false;
            task.invoke();
          },
          () => {
            if (!scheduled) {
              throw new Error(
                "An http observable was completed twice. This shouldn't happen, please file a bug."
              );
            }
            scheduled = false;
            task.invoke();
          }
        );
      };

      const cancelTask = (_task: any) => {
        if (!scheduled) {
          return;
        }
        scheduled = false;
        if (sub) {
          sub.unsubscribe();
          sub = null;
        }
      };

      const onComplete = () => {
        if (savedError !== null) {
          observer.error(savedError);
        } else {
          observer.next(savedResult);
          observer.complete();
        }
      };

      // MockBackend for Http is synchronous, which means that if scheduleTask is by
      // scheduleMacroTask, the request will hit MockBackend and the response will be
      // sent, causing task.invoke() to be called.
      const _task = Zone.current.scheduleMacroTask(
        "ZoneMacroTaskWrapper.subscribe",
        onComplete,
        {},
        () => null,
        cancelTask
      );
      scheduleTask(_task);

      return () => {
        if (scheduled && task) {
          task.zone.cancelTask(task);
          scheduled = false;
        }
        if (sub) {
          sub.unsubscribe();
          sub = null;
        }
      };
    });
  }

  protected abstract delegate(request: S): Observable<R>;
}

/**
 * Mark: 让 universal 等待 API 请求并渲染完毕
 * https://github.com/angular/angular/issues/20520#issuecomment-449597926
 */
export class TaskWrapper extends ZoneMacroTaskWrapper<Promise<any>, any> {
  constructor() {
    super();
  }

  // your public task invocation method signature
  doTask(request: Promise<any>): Observable<any> {
    // call via ZoneMacroTaskWrapper
    return this.wrap(request);
  }

  // delegated raw implementation that will be called by ZoneMacroTaskWrapper
  protected delegate(request: Promise<any>): Observable<any> {
    return new Observable<any>((observer: Observer<any>) => {
      // calling observer.next / complete / error
      request
        .then(result => {
          observer.next(result);
          observer.complete();
        })
        .catch(error => observer.error(error));
    });
  }
}
