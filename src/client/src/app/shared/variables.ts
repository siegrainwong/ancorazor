import { Injectable } from "@angular/core";

/**
 * Injectable root 代表一个单例
 */
@Injectable({
  providedIn: "root"
})
export class Variables {
  renderFromServer: Boolean = false;
}
