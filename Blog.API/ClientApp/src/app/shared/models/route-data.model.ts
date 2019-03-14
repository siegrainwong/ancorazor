export default class RouteData {
  kind: string;
  constructor(obj?: Partial<RouteData>) {
    Object.assign(this, obj);
  }
}
