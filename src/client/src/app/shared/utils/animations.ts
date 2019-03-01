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

export enum headerState {
  Prev = "prev",
  Next = "next"
}

export const headerPrevAnimation = trigger("headerPrevAnimation", [
  state(
    headerState.Prev,
    style({
      opacity: 1
    })
  ),
  state(
    headerState.Next,
    style({
      opacity: 0
    })
  ),
  transition(`${headerState.Prev} <=> ${headerState.Next}`, animate(500))
]);
export const headerNextAnimation = trigger("headerNextAnimation", [
  state(
    headerState.Prev,
    style({
      opacity: 0
    })
  ),
  state(
    headerState.Next,
    style({
      opacity: 1
    })
  ),
  transition(`${headerState.Prev} <=> ${headerState.Next}`, animate(500))
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
