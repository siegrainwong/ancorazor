import { Injectable } from "@angular/core";
import { BaseService, ISGService } from "src/app/shared/services/base.service";
import SiteSettingModel from "../models/site-setting.model";

@Injectable({ providedIn: "root" })
export class CommonService extends BaseService implements ISGService {
  serviceName = "common";
  protected initialize() {}

  async getSetting(): Promise<SiteSettingModel> {
    var res = await this.get(`${this.serviceName}/setting`);
    return res.succeed && res.data;
  }
}
