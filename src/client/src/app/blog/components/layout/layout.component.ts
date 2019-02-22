import { Component, OnInit } from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";

@Component({
  selector: "app-layout",
  templateUrl: "./layout.component.html"
})
export class LayoutComponent implements OnInit {
  kind: string;

  constructor(private router: Router, private route: ActivatedRoute) {
    if (router.url.endsWith("blog/add")) {
      this.kind = "add";
    } else if (router.url.endsWith("blog/about")) {
      this.kind = "about";
    } else if (router.url.includes("blog/article")) {
      this.kind = "article";
    } else {
      this.kind = "home";
    }

    route.url.subscribe(x => {
      console.log("url", x);
    });

    route.params.subscribe(x => {
      console.log("params", x);
    });
  }

  ngOnInit(): void {}
}
