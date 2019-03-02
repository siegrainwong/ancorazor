import {
  Component,
  OnInit,
  Input,
  EventEmitter,
  Output,
  SimpleChanges
} from "@angular/core";
import ArticleModel from "../../models/article-model";
import { Store } from "src/app/shared/store/store";
import * as $ from "jquery";
import { environment } from "src/environments/environment";
import {
  headerPrevAnimation,
  headerNextAnimation,
  headerState
} from "src/app/shared/utils/animations";
@Component({
  selector: "app-header",
  templateUrl: "./header.component.html",
  styleUrls: ["./header.component.scss"],
  animations: [headerPrevAnimation, headerNextAnimation]
})
export class HeaderComponent implements OnInit {
  @Input() state: headerState = headerState.Prev;
  @Input() model: ArticleModel = new ArticleModel();
  @Input() isEditing: boolean = false;
  // 给 write-article 页面用的
  @Output() headerUpdated = new EventEmitter<ArticleModel>();

  constructor(private store: Store) {}

  ngOnInit() {
    this.registerRouteChanged();
  }

  registerRouteChanged() {
    this.store.routeDataChanged$.subscribe(data => {
      if (this.model.cover || this.store.renderFromServer) return;
      switch (data.kind) {
        case "article":
          this.model.cover = "assets/img/article-bg.jpg";
          break;
        case "add":
          this.model.cover = "assets/img/write-bg.jpg";
          break;
        case "home":
          this.model.cover = environment.homeCoverUrl;
          if (this.store.homeCoverLoaded) {
            this.loadCover(false, this.model.cover);
            return;
          } else {
            this.store.homeCoverLoaded = true;
            this.model.cover = environment.homeCoverUrl;
          }
          break;
        default:
          this.model.cover = "assets/img/article-bg.jpg";
          break;
      }

      this.loadCover(true, this.model.cover);
    });
  }

  loadCover(shouldTransition: boolean, src: string) {
    if (shouldTransition) {
      let image = new Image();
      image.onload = function() {
        $(".header-bg.prev").css(
          "background-image",
          "url('" + image.src + "')"
        );
      };
      image.src = src;
    } else {
      $(".header-bg.prev").css("background-image", "url('" + src + "')");
    }
  }

  onTitleBlured(val: string) {
    this.model.title = val;
    this.headerUpdated.emit(this.model);
  }
  onDigestBlured(val: string) {
    this.model.digest = val;
    this.headerUpdated.emit(this.model);
  }
}
