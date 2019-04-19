import { SGAnimation, SGTransitionMode } from "./sg-transition.model";

/**
 * Animation declarations
 * 其中的CSS动画大部分来自 https://daneden.github.io/animate.css/
 */
export const SGAnimations: SGAnimation[] = [
  // route animations
  new SGAnimation({
    name: "fade",
    enterClass: "fadeIn",
    leaveClass: "fadeOut"
  }),
  new SGAnimation({
    name: "fade-up",
    enterClass: "fadeInUp",
    leaveClass: "fadeOutUp"
  }),
  new SGAnimation({
    name: "fade-opposite",
    enterClass: "fadeInUp",
    leaveClass: "fadeOutDown"
  }),

  // custom animations
  new SGAnimation({
    name: "page-turn-next",
    enterClass: "fadeInRight",
    leaveClass: "fadeOutLeft",
    type: SGTransitionMode.custom
  }),
  new SGAnimation({
    name: "page-turn-previous",
    enterClass: "fadeInLeft",
    leaveClass: "fadeOutRight",
    type: SGTransitionMode.custom
  }),
  new SGAnimation({
    name: "page-turn-button",
    enterClass: "fadeInUp",
    leaveClass: "fadeOutDown",
    type: SGTransitionMode.custom
  })
];
