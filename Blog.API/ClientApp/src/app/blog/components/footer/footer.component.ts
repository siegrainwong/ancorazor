import { Component, OnInit, OnDestroy } from "@angular/core";
import { constants } from "src/app/shared/constants/siegrain.constants";
import { SGTransitionDelegate } from "src/app/shared/animations/sg-transition.delegate";
import { SGAnimations } from "src/app/shared/animations/sg-animations";
import { Store } from "src/app/shared/store/store";
import { Subscription } from "rxjs";

@Component({
  selector: "app-footer",
  templateUrl: "./footer.component.html",
  styleUrls: ["./footer.component.scss"]
})
export class FooterComponent
  implements OnInit, OnDestroy, SGTransitionDelegate {
  public animations = {
    footer: SGAnimations.fadeOpposite
  };
  public copyright: string;

  private _subscription = new Subscription();
  constructor(private _store: Store) {}

  ngOnDestroy(): void {
    this._subscription.unsubscribe();
  }

  ngOnInit() {
    this._subscription.add(
      this._store.siteSettingChanged$.subscribe(data => {
        this.copyright = data.copyright;
      })
    );
  }
}
