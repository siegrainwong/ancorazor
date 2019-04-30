import { Injectable, OnDestroy } from "@angular/core";
import { externalScripts } from "../constants/siegrain.constants";
import { SGUtil } from "./siegrain.utils";
import { Store } from "../store/store";
import {
  SGTransitionStore,
  SGTransitionPipeline
} from "../animations/sg-transition.store";
import { Subscription } from "rxjs";

let NProgress: any;

declare global {
  interface Window {
    NProgress: any;
  }
}

const enum ProgressMode {
  manually,
  transition
}

@Injectable({ providedIn: "root" })
export class SGProgress implements OnDestroy {
  private _subsribtion = new Subscription();
  private _mode: ProgressMode;
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

  private async setup() {
    if (!this._store.renderFromClient) return;
    await this._util.loadExternalScripts([externalScripts.nprogress]);
    NProgress = window.NProgress;
    this.progressWithTransition();
  }

  private async progressWithTransition() {
    this._subsribtion.add(
      this._transitionStore.transitionStreamChanged$.subscribe(progress => {
        if (!NProgress) return;
        switch (progress) {
          case SGTransitionPipeline.Ready:
            this._mode = ProgressMode.transition;
            NProgress.start();
            break;
          case SGTransitionPipeline.Complete:
            this.progressDone();
            break;
          default:
            var total = Object.keys(SGTransitionPipeline).length / 2;
            NProgress.set(progress / total);
            break;
        }
      })
    );
  }

  public progressStart() {
    if (!NProgress || this._mode == ProgressMode.transition) return;
    NProgress.start();
    this._mode = ProgressMode.manually;
  }

  public progressDone() {
    if (!NProgress) return;
    NProgress.done();
    this.clear();
  }

  private clear() {
    this._mode = null;
  }
}
