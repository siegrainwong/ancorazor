import { Injectable } from "@angular/core";
import { SGAnimations } from "./sg-animations";
import {
  SGTransitionMode,
  SGTransitionCommands,
  SGCustomizeTransitionCommands,
  SGAnimationData,
  SGCommandData
} from "./sg-transition.model";

@Injectable({
  providedIn: "root"
})
export class SGTransitionUtil {
  /**
   * 获取所有`SGAnimation`的路由引用
   */
  public get getRouteAnimations(): SGAnimationData {
    return this.entriesToObject(
      Object.entries(SGAnimations).filter(
        x => x[1].type == SGTransitionMode.route
      )
    );
  }

  public entriesToObject(entries: [string, any][]): { [name: string]: any } {
    return entries.reduce((acc, cur) => {
      acc[cur[0]] = cur[1];
      return acc;
    }, {});
  }

  public deepCopy(object) {
    return JSON.parse(JSON.stringify(object));
  }

  /** 获取一个动画集合的 duration，总是取集合中最长的 duration 值 */
  public getDuration(animations: SGAnimationData): number {
    const duration = Math.max.apply(
      null,
      Object.values(animations).map(x => x.speed.duration)
    );
    return duration;
  }

  /** 判断是否是自定义命令 */
  private isCustomizeCommands(commands: SGTransitionCommands): boolean {
    return (
      commands["__proto__"].className() === "SGCustomizeTransitionCommands"
    );
  }

  /**
   * 解析命令
   * @internal implementation detail, do not use!
   **/
  public resolveCommands(commands?: SGTransitionCommands): SGCommandData {
    let data: SGCommandData = {
      animations: this.getRouteAnimations,
      scrollTo: commands && commands.scrollTo,
      extraDuration: 0,
      crossRoute: (commands && commands.crossRoute) || false
    };

    if (commands && this.isCustomizeCommands(commands)) {
      const customizeCommands = commands as SGCustomizeTransitionCommands;
      data.animations = customizeCommands.animations;
      data.extraDuration = customizeCommands.extraDuration;
    }

    return data;
  }
}
