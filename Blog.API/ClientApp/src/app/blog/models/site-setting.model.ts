import BaseModel from "src/app/shared/models/base-model";

export default class SiteSettingModel extends BaseModel {
  title: string;
  subtitle?: string;
  coverUrl: string;
  copyright: string;
  siteName: string;
  articleTemplate?: string;

  constructor(init?: Partial<SiteSettingModel>) {
    super(init);
    Object.assign(this, init);
  }
}
