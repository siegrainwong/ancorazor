import { Component, OnInit, OnDestroy } from "@angular/core";
import { SGTransitionDelegate } from "src/app/shared/animations/sg-transition.delegate";
import { SGAnimations } from "src/app/shared/animations/sg-animations";
import { Store } from "src/app/shared/store/store";
import { ObservedComponentBase } from "src/app/shared/components/observed.base";
import { AutoUnsubscribe } from "src/app/shared/utils/auto-unsubscribe.decorator";

@Component({
  selector: "app-footer",
  templateUrl: "./footer.component.html",
  styleUrls: ["./footer.component.scss"]
})
@AutoUnsubscribe()
export class FooterComponent extends ObservedComponentBase
  implements OnInit, OnDestroy, SGTransitionDelegate {
  public animations = {
    footer: SGAnimations.fadeOpposite
  };
  public copyright: string;

  private _settingChanged$;
  constructor(private _store: Store) {
    super();
  }

  ngOnInit() {
    this._settingChanged$ = this._store.siteSettingChanged$.subscribe(data => {
      this.copyright = data.copyright;
    });
  }
}
