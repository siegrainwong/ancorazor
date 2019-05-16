/**
 * Auto unsubscribe when components destroyed in order to release memory.
 * @param blackList properties you don't want to unsubscribe when destroyed.
 *
 * MARK: The best way to unsubscribe
 * https://blog.angularindepth.com/the-best-way-to-unsubscribe-rxjs-observable-in-the-angular-applications-d8f9aa42f6a0
 */
export function AutoUnsubscribe(blackList = []) {
  return function(constructor) {
    const original = constructor.prototype.ngOnDestroy;

    constructor.prototype.ngOnDestroy = function() {
      for (let prop in this) {
        const property = this[prop];
        if (blackList.includes(prop)) continue;
        if (property && typeof property.unsubscribe === "function") {
          property.unsubscribe();
        }
      }
      original &&
        typeof original === "function" &&
        original.apply(this, arguments);
    };
  };
}
