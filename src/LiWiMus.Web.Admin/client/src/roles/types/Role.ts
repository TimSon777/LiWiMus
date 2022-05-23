import {Permission} from "../../permissions/Permission";
import {parseISO} from "date-fns";
import {SystemPermission} from "../../systemPermissions/SystemPermission";

export class Role {
  id: number;
  name: string;
  description: string;
  permissions: SystemPermission[];

  createdAt: Date;
  modifiedAt: Date;

  constructor(
    id: string,
    name: string,
    description: string,
    pricePerMonth: string,
    permissions: any[],
    createdAt: string,
    modifiedAt: string
  ) {
    this.id = +id;
    this.name = name;
    this.description = description;

    this.permissions = permissions.map(
      (obj) =>
        new Permission(
          obj.id,
          obj.name,
          obj.description,
          obj.createdAt,
          obj.modifiedAt
        )
    );

    this.createdAt = parseISO(createdAt);
    this.modifiedAt = parseISO(modifiedAt);
  }
}