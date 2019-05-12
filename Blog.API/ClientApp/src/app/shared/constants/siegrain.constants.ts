import { random } from "src/app/shared/utils/random";

export const constants = {
  title: "siegrain🌌wang",
  titlePlainText: "siegrain.wang",
  homeCoverUrl: `assets/img/bg${random(1, 7)}.jpg`
};

export const externalScripts = {
  // https://github.com/Ionaru/easy-markdown-editor
  editor: "assets/libraries/easymde.min.js",
  /**
   * 我定制的 highlight.js，部分语言不支持
   * 定制地址：https://highlightjs.org/download/
   */
  highlight: "assets/libraries/highlight.pack.js",
  marked: "assets/libraries/marked.min.js",
  nprogress: "assets/libraries/nprogress.min.js",
  filepond: "https://unpkg.com/filepond/dist/filepond.js",
  filepondSizeValidation:
    "https://unpkg.com/filepond-plugin-file-validate-size/dist/filepond-plugin-file-validate-size.js",
  filepondResize:
    "https://unpkg.com/filepond-plugin-image-resize/dist/filepond-plugin-image-resize.js"
};

export const articleDefaultContent = `---
title: Enter your title here.
categories:
- development
tags:
- dotnet
- dotnet core
---

**Hello world!**`;
