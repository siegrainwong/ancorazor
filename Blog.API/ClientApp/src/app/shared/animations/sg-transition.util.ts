import { Injectable } from "@angular/core";
import { SGAnimations } from "./sg-animations";
import {
  SGTransitionMode,
  SGAnimation,
  TransitionCommands,
  CustomizeTransitionCommands,
  SGAnimationData
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
    // const getCircularReplacer = () => {
    //   const seen = new WeakSet();
    //   return (key, value) => {
    //     if (typeof value === "object" && value !== null) {
    //       if (seen.has(value)) return;
    //       seen.add(value);
    //     }
    //     return value;
    //   };
    // };
    // return JSON.parse(JSON.stringify(object, getCircularReplacer()));
    return JSON.parse(JSON.stringify(object));
  }

  /** 判断是否是自定义命令 */
  private isCustomizeCommands(commands: TransitionCommands): boolean {
    return commands.constructor.name === CustomizeTransitionCommands.name;
  }

  /**
   * 解析命令
   * @internal implementation detail, do not use!
   **/
  public resolveCommands(
    commands?: TransitionCommands
  ): {
    animations: SGAnimationData;
    extraDuration: number;
    scrollTo?: string;
    crossRoute: boolean;
  } {
    let data = {
      animations: this.getRouteAnimations,
      scrollTo: commands && commands.scrollTo,
      extraDuration: 0,
      crossRoute: (commands && commands.crossRoute) || false
    };

    if (commands && this.isCustomizeCommands(commands)) {
      const customizeCommands = commands as CustomizeTransitionCommands;
      data.animations = customizeCommands.animations;
      data.extraDuration = customizeCommands.extraDuration;
    }

    return data;
  }
}
