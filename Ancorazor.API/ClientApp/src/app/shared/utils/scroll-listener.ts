let previousTop = 0;
function getHeaderHeight(): number {
  const header = document.querySelector("#header");
  return (header && header.clientHeight) || 0;
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
  let nav = getNav();
  const headerHeight = getHeaderHeight();
  if (!headerHeight) {
    nav.classList.add("is-visible", "is-fixed");
    return;
  }

  let currentTop = getScrollTop();
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
