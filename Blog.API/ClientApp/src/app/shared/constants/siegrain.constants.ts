import { random } from "src/app/shared/utils/random";

export const constants = {
  title: "siegrain🌌wang",
  titlePlainText: "siegrain.wang",
  homeCoverUrl: `assets/img/bg${random(1, 7)}.jpg`,
  enableAnimation: true
};

export const externalScripts = {
  // https://github.com/Ionaru/easy-markdown-editor
  editor: "assets/libraries/editor.min.js",
  /**
   * 我定制的 highlight.js，部分语言不支持
   * 定制地址：https://highlightjs.org/download/
   */
  highlight: "assets/libraries/highlight.pack.js"
};
