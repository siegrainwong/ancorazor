export default class BaseModel {
  id: number;
  updatedAt?: Date;
  createdAt: Date;
  remark?: string;
  tags?: string[];
  categories?: string[];
  isDeleted?: boolean;

  constructor(init?: Partial<BaseModel>) {
    Object.assign(this, init);
  }
}
