import { Injectable } from "@angular/core";
import { SGAnimations } from "./sg-animations";
import {
  SGTransitionMode,
  SGAnimation,
  TransitionCommands,
  CustomizeTransitionCommands
} from "./sg-transition.model";

@Injectable({
  providedIn: "root"
})
export class SGTransitionUtil {
  /**
   * 获取所有`SGAnimation`的路由引用
   */
  public get getRouteAnimations(): Array<SGAnimation> {
    return SGAnimations.filter(x => x.type == SGTransitionMode.route);
  }

  /**
   * 根据动画名称获取`SGAnimation`引用集合
   * @param names 动画名称集合
   */
  public getAnimations(names: string[]): Array<SGAnimation> {
    return SGAnimations.filter(x => names.indexOf(x.name) > -1);
  }

  /**
   * 从已有动画定义中返回一个 `SGAnimation` 对象的一个**引用**
   * @param name 动画名称
   */
  public getAnimation(name: string): SGAnimation {
    return this.getAnimations([name])[0];
  }

  /** 判断是否是自定义命令 */
  public isCustomizeCommands(commands: TransitionCommands): boolean {
    return commands.constructor.name === CustomizeTransitionCommands.name;
  }

  /** 解析命令 */
  public resolveCommands(
    commands?: TransitionCommands
  ): {
    animations: SGAnimation[];
    extraDuration: number;
    scrollTo?: string;
  } {
    let data = {
      animations: this.getRouteAnimations,
      scrollTo: null,
      extraDuration: 0
    };

    if (commands && this.isCustomizeCommands(commands)) {
      const customizeCommands = commands as CustomizeTransitionCommands;
      data.animations = this.getAnimations(customizeCommands.names);
      data.scrollTo = customizeCommands.scrollTo;
      data.extraDuration = customizeCommands.extraDuration;
    }

    return data;
  }
}
