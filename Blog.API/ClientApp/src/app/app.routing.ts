import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { DemoComponent } from "./blog/components/demo/demo.component";

const routes: Routes = [
  // { path: "", loadChildren: "./blog/blog.module#BlogModule" },
  {
    path: "",
    component: DemoComponent
  },
  { path: "**", redirectTo: "" }
];
@NgModule({
  // https://github.com/angular/angular/issues/15716
  // Mark: 修复从 Server render 到 Client render 的闪烁
  imports: [RouterModule.forRoot(routes, { initialNavigation: "enabled" })],
  exports: [RouterModule]
})
export class AppRoutingModule {}
