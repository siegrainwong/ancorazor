import { Injectable, OnDestroy } from "@angular/core";
import {
  Resolve,
  RouterStateSnapshot,
  ActivatedRouteSnapshot
} from "@angular/router";
import { Observable, EMPTY, Subscription } from "rxjs";
import { SGTransitionStore } from "./sg-transition.store";
import { take } from "rxjs/operators";
import { timeout } from "../utils/promise-delay";
import {
  SGCustomTransitionDelegate as SGCustomizeTransitionDelegate,
  SGTransitionDelegate
} from "./sg-transition.interface";
import {
  RouteTransitionCommands,
  SGTransitionMode,
  TransitionCommands
} from "./sg-transition.model";
import { SGTransition } from "./sg-transition";
import { SGUtil } from "../utils/siegrain.utils";
import { Store } from "../store/store";

@Injectable({
  providedIn: "root"
})
export class SGTransitionResolveGuard implements Resolve<boolean>, OnDestroy {
  private _subscription = new Subscription();
  constructor(
    private _transitionStore: SGTransitionStore,
    private _transition: SGTransition,
    private _util: SGUtil,
    private _store: Store
  ) {}

  async resolve(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Promise<boolean> {
    if (this._store.isFirstScreen || !this._store.renderFromClient) return true;

    const resolveCount = this.getResolveCount(route);
    if (!resolveCount) {
      console.log("nothing resolved before transition");
      await this.transition(route);
      return true;
    }

    console.log(`${resolveCount} resolves executing`);
    return new Promise(resolve => {
      this._subscription.add(
        this._transitionStore.resolveCompletedChanged$
          .pipe(take(resolveCount))
          .subscribe(async number => {
            if (number != resolveCount) return;
            console.log(`${number} resolves completed`);
            await this.transition(route);
            resolve(true);
          })
      );
    });
  }

  private async transition(route: ActivatedRouteSnapshot) {
    const url = route.url.toString();
    let delegates = this.setupDelegate(
      this._transitionStore.transitionDelegate
    );
    if (delegates.customDelegate)
      await this.transitionWithCustomMode(delegates.customDelegate, url);
    else await this.transitionWithRouteMode(delegates.routeDelegate, url);
    this._transitionStore.setTransitioned();
  }

  /* 执行路由过渡动画 */
  private async transitionWithRouteMode(
    delegate: SGTransitionDelegate,
    url: string
  ) {
    let commands =
      (delegate.RouteTransitionForComponent &&
        delegate.RouteTransitionForComponent(delegate, url)) ||
      new RouteTransitionCommands();

    this.scroll(commands);
    await this._transition.triggerTransition();
  }

  /* 执行自定义过渡动画 */
  private async transitionWithCustomMode(
    delegate: SGCustomizeTransitionDelegate,
    url: string
  ) {
    const mode = delegate.ModeForComponentTransition(delegate, url);
    if (mode == SGTransitionMode.route)
      return this.transitionWithRouteMode(delegate, url);

    const commands = delegate.CustomizeTransitionForComponent(delegate, url);
    this.scroll(commands);
    await this._transition.triggerTransition(
      commands.names,
      commands.extraDuration
    );
  }

  private scroll(commands: TransitionCommands) {
    commands.scrollTo && this._util.scrollTo(commands.scrollTo);
  }

  /* 根据当前页面实现的接口返回对应的代理 */
  private setupDelegate(
    component: SGTransitionDelegate
  ): {
    customDelegate?: SGCustomizeTransitionDelegate;
    routeDelegate?: SGTransitionDelegate;
  } {
    let customized = <SGCustomizeTransitionDelegate>(<unknown>component);
    if (
      customized.ModeForComponentTransition &&
      customized.CustomizeTransitionForComponent
    ) {
      return {
        customDelegate: customized
      };
    }

    return {
      routeDelegate: component
    };
  }

  private getResolveCount(route: ActivatedRouteSnapshot): number {
    const resolveKey = "_resolve";
    return (
      (route[resolveKey] && Object.keys(route[resolveKey]).length - 1) || 0
    );
  }

  ngOnDestroy(): void {
    this._subscription.unsubscribe();
  }
}

@Injectable({
  providedIn: "root"
})
export class SGBaseResolveGuard<TOutput> implements Resolve<TOutput> {
  constructor(private _transitionStore: SGTransitionStore) {}
  resolve(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<TOutput> {
    console.log("SGBaseResolveGuard");
    this._transitionStore.setResolved();
    return EMPTY;
  }
}
