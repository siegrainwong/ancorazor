# SGTransition 过渡动画模块说明

`SGTransition`是一个基于样式的动画过渡模块，大多用于组件入场出场的过渡动画。

## 文件说明

| 文件                              | 说明                                                    |
| --------------------------------- | ------------------------------------------------------- |
| sg-transition.ts                  | 过渡核心                                                |
| sg-transition.enter.ts            | 入场过渡核心                                            |
| sg-transition.leave.ts            | 离场过渡核心                                            |
| sg-animations.ts                  | 过渡动画声明                                            |
| sg-transition.model.ts            | 模型                                                    |
| sg-transition.util.ts             | 帮助类                                                  |
| sg-transition.store.ts            | 状态管理                                                |
| sg-transition.delegate.ts         | 代理，用于需要额外配置的`components`使用                |
| sg-transition.deactivate.guard.ts | 配置该`guard`到需要过渡动画的组件路由的`canDeativate`上 |
| sg-transition.resolve.guard.ts    | 配置该`guard`到需要过渡动画的组件路由的`resolve`上      |
