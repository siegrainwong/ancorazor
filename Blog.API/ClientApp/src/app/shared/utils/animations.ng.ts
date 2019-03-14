import {
  animation,
  trigger,
  group,
  transition,
  animate,
  style,
  query,
  state,
  stagger
} from "@angular/animations";

/**
 * 没有用。
 * Angular 的 animation api 太废了。。
 */

export const slideInAnimation = trigger("routeAnimations", [
  transition("home <=> about", [
    query(":leave", [
      style({
        transform: "translateY(0%)",
        opacity: 1,
        position: "absolute",
        top: 0,
        width: "100%"
      })
    ]),
    query(":enter", [
      style({
        transform: "translateY(100%)",
        opacity: 0
      })
    ]),
    group([
      query(":leave .site-heading", [
        animate(
          "5s ease-out",
          style({ transform: "translateY(-100%)", opacity: 0 })
        )
      ]),
      query(":enter .site-heading", [
        animate(
          "5s ease-out",
          style({ transform: "translateY(0)", opacity: 1 })
        )
      ])
    ])
  ])
]);

export const listAnimation = trigger("listAnimation", [
  transition(":enter, * => 0, * => -1", []),
  transition(":increment", [
    query(
      ":enter",
      [
        style({ opacity: 0, width: "0px" }),
        stagger(50, [
          animate("300ms ease-out", style({ opacity: 1, width: "*" }))
        ])
      ],
      { optional: true }
    )
  ]),
  transition(":decrement", [
    query(":leave", [
      stagger(50, [
        animate("300ms ease-out", style({ opacity: 0, width: "0px" }))
      ])
    ])
  ])
]);
