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

export enum HeaderState {
  Prev = "headerPrevState",
  Next = "headerNextState"
}

export const headerPrevAnimation = trigger(HeaderState.Prev, [
  state(
    "prev",
    style({
      opacity: 1
    })
  ),
  state(
    "next",
    style({
      opacity: 0
    })
  ),
  transition("prev <=> next", animate(500))
]);
export const headerNextAnimation = trigger(HeaderState.Next, [
  state(
    "prev",
    style({
      opacity: 0
    })
  ),
  state(
    "next",
    style({
      opacity: 1
    })
  ),
  transition("next <=> prev", animate(500))
]);

export const slideInAnimation = trigger("routeAnimations", [
  // transition("home <=> article", [
  //   // query(":enter .anim-container, :leave .anim-container", [
  //   //   style({
  //   //     position: "absolute",
  //   //     left: 0,
  //   //     top: 500,
  //   //     width: "100%"
  //   //   })
  //   // ]),
  //   // query(":enter app-header, .masthead", [
  //   //   style({
  //   //     position: "absolute",
  //   //     top: 0,
  //   //     width: "100%"
  //   //   })
  //   // ]),
  //   // query(":enter .anim-container", [style({ left: "-100%" })]),
  //   query(".masthead", [style({ opacity: "0" })]),
  //   query(":leave .anim-container, .masthead", animateChild()),
  //   group([
  //     // query(":leave .anim-container", [
  //     //   animate("300ms ease-out", style({ left: "100%" }))
  //     // ]),
  //     query(".masthead", [
  //       animate("300ms ease-out", style({ opacity: "0" }))
  //     ]),
  //     // query(":enter .anim-container", [
  //     //   animate("300ms ease-out", style({ left: "0%" }))
  //     // ]),
  //     query(":enter app-header", [
  //       animate("300ms ease-out", style({ opacity: "1" }))
  //     ])
  //   ]),
  //   query(":enter .anim-container, :enter app-header", animateChild())
  // ])
]);
