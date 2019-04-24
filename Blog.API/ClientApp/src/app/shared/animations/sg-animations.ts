import { SGAnimation, SGTransitionMode } from "./sg-transition.model";

/**
 * Animation declarations
 * 其中的CSS动画大部分来自 https://daneden.github.io/animate.css/
 */
export const SGAnimations = {
  // route animations
  fade: new SGAnimation({
    enterClass: "fadeIn",
    leaveClass: "fadeOut"
  }),
  fadeUp: new SGAnimation({
    enterClass: "fadeInUp",
    leaveClass: "fadeOutUp"
  }),
  fadeOpposite: new SGAnimation({
    enterClass: "fadeInUp",
    leaveClass: "fadeOutDown"
  }),

  // custom animations
  pageTurnNext: new SGAnimation({
    enterClass: "fadeInRight",
    leaveClass: "fadeOutLeft",
    type: SGTransitionMode.custom
  }),
  pageTurnPrevious: new SGAnimation({
    enterClass: "fadeInLeft",
    leaveClass: "fadeOutRight",
    type: SGTransitionMode.custom
  }),
  pageTurnButton: new SGAnimation({
    enterClass: "fadeInUp",
    leaveClass: "fadeOutDown",
    type: SGTransitionMode.custom
  })
};
