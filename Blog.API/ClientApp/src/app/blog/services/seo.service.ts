import { Injectable } from "@angular/core";
import { Meta } from "@angular/platform-browser";

@Injectable({ providedIn: "root" })
export class SEOService {
  constructor(private meta: Meta) {
    meta.addTag({
      name: "description",
      content: "Title and Meta tags examples"
    });
    meta.addTag({ name: "author", content: "ABCD" });
    meta.addTag({ name: "keywords", content: "TypeScript, Angular" });
    meta.addTag({ name: "date", content: "2018-06-02", scheme: "YYYY-MM-DD" });
    meta.addTag({ charset: "UTF-8" });
    meta.addTag({ property: "og:site_name", content: "siegrain.wang" });
  }
}
