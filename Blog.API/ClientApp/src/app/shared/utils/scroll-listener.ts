let previousTop = 0;
function getHeaderHeight(): number {
  return document.querySelector("#header").clientHeight;
}
function getNav(): HTMLElement {
  return document.querySelector("#mainNav");
}
function getScrollTop(): number {
  return (
    window.pageYOffset ||
    document.documentElement.scrollTop ||
    document.body.scrollTop ||
    0
  );
}
/** 监听滚动 隐藏/显示 nav */
export function onScroll() {
  let currentTop = getScrollTop();
  let nav = getNav();
  //check if user is scrolling up
  if (currentTop < previousTop) {
    //if scrolling up...
    if (currentTop > 0 && nav.classList.contains("is-fixed")) {
      nav.classList.add("is-visible");
    } else {
      nav.classList.remove("is-visible", "is-fixed");
    }
  } else if (currentTop > previousTop) {
    //if scrolling down...
    nav.classList.remove("is-visible");
    if (currentTop > getHeaderHeight() && !nav.classList.contains("is-fixed"))
      nav.classList.add("is-fixed");
  }
  previousTop = currentTop;
}
