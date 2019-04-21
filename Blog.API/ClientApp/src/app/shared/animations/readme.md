# SGTransition 过渡动画模块说明

| 文件                              | 说明                                                    |
| --------------------------------- | ------------------------------------------------------- |
| sg-transition.ts                  | 过渡核心                                                |
| sg-animations.ts                  | 过渡动画声明                                            |
| sg-transition.model.ts            | 模型                                                    |
| sg-transition.store.ts            | 状态管理                                                |
| sg-transition.delegate.ts         | 代理，用于需要额外配置的`components`使用                |
| sg-transition.deactivate.guard.ts | 配置该`guard`到需要过渡动画的组件路由的`canDeativate`上 |
| sg-transition.resolve.guard.ts    | 配置该`guard`到需要过渡动画的组件路由的`resolve`上      |

> 注：目前虽然所有脚本文件是独立成模块了，但依然有一些样式依赖，暂时还不能复用到其他项目中。
