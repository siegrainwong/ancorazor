import { random } from "src/app/shared/utils/random";

export const constants = {
  homeCoverUrl: `assets/img/bg${random(1, 7)}.jpg`
};

export const coverSize = {
  width: 1600,
  height: 1200,
  ratio: "4:3"
};

export const externalScripts = {
  // https://github.com/Ionaru/easy-markdown-editor
  editor: "assets/libraries/easymde.min.js",
  /**
   * customized highlight.js
   * ref http://highlightjs.org/download/
   */
  highlight: "assets/libraries/highlight.pack.js",
  marked: "assets/libraries/marked.min.js",
  nprogress: "assets/libraries/nprogress.min.js",
  filepond: "assets/libraries/filepond/filepond.min.js",
  filepondSizeValidation:
    "assets/libraries/filepond/filepond-plugin-file-validate-size.min.js",
  filepondResize:
    "assets/libraries/filepond/filepond-plugin-image-resize.min.js",
  filepondTransform:
    "assets/libraries/filepond/filepond-plugin-image-transform.min.js",
  filepondCrop: "assets/libraries/filepond/filepond-plugin-image-crop.min.js",
  filepondTypeValidation:
    "assets/libraries/filepond/filepond-plugin-file-validate-type.min.js",
  gitment: "assets/libraries/gitment.browser.js"
};

export const articleDefaultContent = `---
title: Enter your title here.
category: development
tags:
- dotnet
- dotnet core
---

**Hello world!**`;
