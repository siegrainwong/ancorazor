import { Injectable } from "@angular/core";
import { BaseService, ISubService } from "src/app/shared/services/base.service";
import SiteSettingModel from "../models/site-setting.model";

@Injectable({ providedIn: "root" })
export class CommonService extends BaseService implements ISubService {
  serviceName = "common";
  protected initialize() {}

  async getSetting(): Promise<SiteSettingModel> {
    var res = await this.get(`${this.serviceName}/setting`);
    return res.succeed && res.data;
  }
}
