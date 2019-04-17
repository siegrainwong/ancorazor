import { Injectable } from "@angular/core";
import {
  Resolve,
  ActivatedRouteSnapshot,
  RouterStateSnapshot
} from "@angular/router";
import { timeout } from "../utils/promise-delay";
import BaseModel from "../models/base-model";

@Injectable({
  providedIn: "root"
})
export class SGBaseResolver implements Resolve<BaseModel> {
  async resolve(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Promise<BaseModel> {
    console.log("animation resolver begin");
    await timeout(2000);
    console.log("animation resolver end");
    return;
  }
}
