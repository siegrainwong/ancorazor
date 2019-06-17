import { OnDestroy, Component, Injectable } from "@angular/core";

/**
 * Used for `@AutoUnsubscribe()` decorator
 * You must implement `OnDestroy` explicitly for AoT reason.
 */
@Component({
  selector: "app-observed-base",
  template: ""
})
export class ObservedComponentBase implements OnDestroy {
  ngOnDestroy(): void {}
}

/**
 * Used for `@AutoUnsubscribe()` decorator
 * You must implement `OnDestroy` explicitly for AoT reason.
 */
export class ObservedServiceBase implements OnDestroy {
  ngOnDestroy(): void {}
}
