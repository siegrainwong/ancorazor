import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Router, NavigationEnd } from "@angular/router";
import { filter } from "rxjs/operators";
import { Store } from "../shared/store/store";
import RouteData from "../shared/models/route-data.model";
import { SGTransition } from "../shared/utils/siegrain.animations";
import { SGUtil } from "../shared/utils/siegrain.utils";
import { externalScripts } from "../shared/constants/siegrain.constants";

@Component({
  selector: "app-blog-app",
  templateUrl: "./blog-app.component.html"
})
export class BlogAppComponent implements OnInit {
  constructor(
    private _route: ActivatedRoute,
    private _router: Router,
    private _util: SGUtil,
    public store: Store,
    public transition: SGTransition
  ) {}

  ngOnInit() {
    this.loadExternalResources();
    this.observeRoute();
    this.store.setupUser();
  }

  private loadExternalResources() {
    if (!this.store.renderFromClient) return;
    this._util.loadExternalScripts(externalScripts.tuiEditor);
  }

  private observeRoute() {
    this.store.routeData = this._route.firstChild.snapshot.data as RouteData;
    this._router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe(() => {
        this.store.routeData = this._route.firstChild.snapshot
          .data as RouteData;
      });
  }
}
