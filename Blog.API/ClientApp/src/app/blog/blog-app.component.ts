import { Component, OnInit } from "@angular/core";
import {
  ActivatedRoute,
  Router,
  NavigationEnd,
  RouterOutlet
} from "@angular/router";
import { filter } from "rxjs/operators";
import { Store } from "../shared/store/store";
import RouteData from "../shared/models/route-data.model";
import { SGTransition } from "../shared/utils/siegrain.animations";

@Component({
  selector: "app-blog-app",
  templateUrl: "./blog-app.component.html"
})
export class BlogAppComponent implements OnInit {
  constructor(
    private _route: ActivatedRoute,
    private _router: Router,
    public store: Store,
    public transition: SGTransition
  ) {}

  ngOnInit() {
    this.observeRoute();
    this.store.setupUser();
  }

  prepareRoute(outlet: RouterOutlet) {
    return (
      outlet && outlet.activatedRouteData && outlet.activatedRouteData["kind"]
    );
  }

  observeRoute() {
    this.store.routeData = this._route.firstChild.snapshot.data as RouteData;
    this._router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe(() => {
        this.store.routeData = this._route.firstChild.snapshot
          .data as RouteData;
      });
  }
}
