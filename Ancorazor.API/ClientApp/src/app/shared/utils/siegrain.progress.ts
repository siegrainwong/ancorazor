import { Injectable, OnDestroy } from "@angular/core";
import { externalScripts } from "../constants/siegrain.constants";
import { SGUtil } from "./siegrain.utils";
import { Store } from "../store/store";
import {
  SGTransitionStore,
  SGTransitionPipeline
} from "../animations/sg-transition.store";
import { Subscription } from "rxjs";
import { ObservedServiceBase } from "../components/observed.base";
import { AutoUnsubscribe } from "./auto-unsubscribe.decorator";

declare global {
  interface Window {
    NProgress: any;
  }
}

export const enum SGProgressMode {
  manually,
  transition
}

/**
 * 进度条
 *
 * ref: https://github.com/rstacruz/nprogress
 */
@Injectable({ providedIn: "root" })
@AutoUnsubscribe()
export class SGProgress extends ObservedServiceBase implements OnDestroy {
  private _mode: SGProgressMode;
  private _streamChanged$;
  constructor(
    private _util: SGUtil,
    private _store: Store,
    private _transitionStore: SGTransitionStore
  ) {
    super();
    this._store.renderFromClient && this.setup();
  }

  private get isAvailable() {
    return this._store.renderFromClient && window && window.NProgress;
  }

  async setup() {
    await this._util.loadExternalScripts([externalScripts.nprogress]);
    window.NProgress.configure({
      showSpinner: false
    });
    this.progressWithTransition();
  }

  private async progressWithTransition() {
    this._streamChanged$ = this._transitionStore.transitionStreamChanged$.subscribe(
      progress => {
        if (!this.isAvailable) return;
        switch (progress) {
          case SGTransitionPipeline.Ready:
            this._mode = SGProgressMode.transition;
            window.NProgress.start();
            break;
          case SGTransitionPipeline.Complete:
            this.progressDone(SGProgressMode.transition);
            break;
          default:
            if (this._mode != SGProgressMode.transition) return;
            var total = Object.keys(SGTransitionPipeline).length / 2;
            window.NProgress.set(progress / total);
            break;
        }
      }
    );
  }

  public progressStart() {
    if (!this.isAvailable || this._mode == SGProgressMode.transition) return;
    window.NProgress.start();
    this._mode = SGProgressMode.manually;
  }

  public progressDone(mode: SGProgressMode) {
    if (!this.isAvailable) return;
    if (this._mode !== mode) return;
    window.NProgress.done();
    this.clear();
  }

  private clear() {
    this._mode = null;
  }
}
