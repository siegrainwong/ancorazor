import {
  animation,
  trigger,
  animateChild,
  group,
  transition,
  animate,
  style,
  query,
  state
} from "@angular/animations";

export enum headerAnimationState {
  prev = "prev",
  next = "next"
}

/**
 * Mark: header 背景动画
 * https://stackoverflow.com/questions/49158271/angular-animations-with-query-not-keeping-state
 * https://github.com/angular/angular/issues/18775#issuecomment-323469531
 */

// export const headerPrevAnimation = trigger("headerPrevAnimation", [
//   state(
//     headerAnimationState.prev,
//     style({
//       opacity: 1,
//       transform: "translateX(0)"
//     })
//   ),
//   state(
//     headerAnimationState.next,
//     style({
//       opacity: 0,
//       transform: "translateX(-100%)"
//     })
//   ),
//   transition(
//     `${headerAnimationState.prev} <=> ${headerAnimationState.next}`,
//     animate(500)
//   )
// ]);
// export const headerNextAnimation = trigger("headerNextAnimation", [
//   state(
//     headerAnimationState.prev,
//     style({
//       opacity: 0,
//       transform: "translateX(-100%)"
//     })
//   ),
//   state(
//     headerAnimationState.next,
//     style({
//       opacity: 1,
//       transform: "translateX(0)"
//     })
//   ),
//   transition(
//     `${headerAnimationState.prev} <=> ${headerAnimationState.next}`,
//     animate(500)
//   )
// ]);

// transform: "translateX(-100%)"

// TODO: 不知道为什么动画就是不起作用
export const slideInAnimation = trigger("routeAnimations", [
  transition("home <=> about", [
    style({ position: "relative" }),
    query(":enter, :leave", [
      style({
        position: "absolute",
        top: 0,
        left: 0,
        width: "100%"
      })
    ]),
    query(":enter", [style({ left: "-100%" })]),
    query(":leave", animateChild()),
    group([
      query(":leave", [animate("5s ease-out", style({ left: "100%" }))]),
      query(":enter", [animate("5s ease-out", style({ left: "0%" }))])
    ]),
    query(":enter", animateChild())
  ])
]);
