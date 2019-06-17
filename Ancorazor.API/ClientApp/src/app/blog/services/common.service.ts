import { Injectable } from "@angular/core";
import { BaseService, ISGService } from "src/app/shared/services/base.service";
import SiteSettingModel from "../models/site-setting.model";

@Injectable({ providedIn: "root" })
export class CommonService extends BaseService implements ISGService {
  serviceName = "common";
  protected initialize() {}

  async getSetting(): Promise<SiteSettingModel> {
    const res = await this.get(`${this.serviceName}/setting`);
    if (!res.succeed) return null;
    let model = res.data as SiteSettingModel;
    if (model.gitment)
      model.gitment = super.deserializeJsonFromBackend(model.gitment);
    return model;
  }
}
