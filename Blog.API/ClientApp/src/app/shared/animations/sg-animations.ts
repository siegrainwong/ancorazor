import { SGTransitionMode, SGFadeAnimation } from "./sg-transition.model";

/**
 * Animation declarations
 * 其中的CSS动画大部分来自 https://daneden.github.io/animate.css/
 */
export const SGAnimations = {
  // route animations
  fade: new SGFadeAnimation({
    enterClass: "fadeIn",
    leaveClass: "fadeOut"
  }),
  fadeUp: new SGFadeAnimation({
    enterClass: "fadeInUp",
    leaveClass: "fadeOutUp"
  }),
  fadeOpposite: new SGFadeAnimation({
    enterClass: "fadeInUp",
    leaveClass: "fadeOutDown"
  }),

  // custom animations
  pageTurnNext: new SGFadeAnimation({
    enterClass: "fadeInRight",
    leaveClass: "fadeOutLeft",
    type: SGTransitionMode.custom
  }),
  pageTurnPrevious: new SGFadeAnimation({
    enterClass: "fadeInLeft",
    leaveClass: "fadeOutRight",
    type: SGTransitionMode.custom
  }),
  pageTurnButton: new SGFadeAnimation({
    enterClass: "fadeInUp",
    leaveClass: "fadeOutDown",
    type: SGTransitionMode.custom
  })
};
