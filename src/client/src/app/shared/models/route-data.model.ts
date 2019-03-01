export default class RouteData {
  kind: string;
  animation?: string;
  constructor(kind: string, animation?: string) {
    this.kind = kind;
    this.animation = animation;
  }
}
