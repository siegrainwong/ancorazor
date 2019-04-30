import { Injectable, OnDestroy } from "@angular/core";
import { externalScripts } from "../constants/siegrain.constants";
import { SGUtil } from "./siegrain.utils";
import { Store } from "../store/store";
import {
  SGTransitionStore,
  SGTransitionPipeline
} from "../animations/sg-transition.store";
import { Subscription } from "rxjs";

declare global {
  interface Window {
    NProgress: any;
  }
}

const enum SGProgressMode {
  manually,
  transition
}

/**
 * 进度条
 *
 * ref: https://github.com/rstacruz/nprogress
 */
@Injectable({ providedIn: "root" })
export class SGProgress implements OnDestroy {
  private _subsribtion = new Subscription();
  private _mode: SGProgressMode;
  constructor(
    private _util: SGUtil,
    private _store: Store,
    private _transitionStore: SGTransitionStore
  ) {
    this.setup();
  }

  ngOnDestroy() {
    this._subsribtion.unsubscribe();
  }

  private get isAvailable() {
    return this._store.renderFromClient && window && window.NProgress;
  }

  private async setup() {
    if (!this._store.renderFromClient) return;
    await this._util.loadExternalScripts([externalScripts.nprogress]);
    window.NProgress.configure({ showSpinner: false });
    this.progressWithTransition();
  }

  private async progressWithTransition() {
    this._subsribtion.add(
      this._transitionStore.transitionStreamChanged$.subscribe(progress => {
        if (!this.isAvailable) return;
        switch (progress) {
          case SGTransitionPipeline.Ready:
            this._mode = SGProgressMode.transition;
            window.NProgress.start();
            break;
          case SGTransitionPipeline.Complete:
            this.progressDone();
            break;
          default:
            var total = Object.keys(SGTransitionPipeline).length / 2;
            window.NProgress.set(progress / total);
            break;
        }
      })
    );
  }

  public progressStart() {
    if (!this.isAvailable || this._mode == SGProgressMode.transition) return;
    window.NProgress.start();
    this._mode = SGProgressMode.manually;
  }

  public progressDone() {
    if (!this.isAvailable) return;
    window.NProgress.done();
    this.clear();
  }

  private clear() {
    this._mode = null;
  }
}
