import { random } from "src/app/shared/utils/random";

export const constants = {
  title: "siegrain🌌wang",
  titlePlainText: "siegrain.wang",
  homeCoverUrl: `assets/img/bg${random(1, 7)}.jpg`,
  enableAnimation: true
};

export const externalScripts = {
  // tuiEditor: "assets/libraries/tui.editor/tui-editor-Editor-full.min.js"
  tuiEditor:
    "assets/libraries/tui.editor/tui-editor-Editor-full.js",
  // tuiEditorScrollSync: "assets/libraries/tui.editor/tui-editor-extScrollSync.js"
};

// export const externalStyles = {
//   tuiEditor: "assets/libraries/tui.editor/tui-editor.min.css"
// };
