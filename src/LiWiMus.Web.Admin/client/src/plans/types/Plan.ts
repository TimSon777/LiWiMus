import { Permission } from "../../permissions/Permission";
import { parseISO } from "date-fns";

export class Plan {
  id: number;
  name: string;
  description: string;
  pricePerMonth: number;
  permissions: Permission[];

  createdAt: Date;
  modifiedAt: Date;

  deletable: boolean;

  constructor(
    id: string,
    name: string,
    description: string,
    pricePerMonth: string,
    permissions: any[],
    createdAt: string,
    modifiedAt: string,
    deletable: string
  ) {
    this.id = +id;
    this.name = name;
    this.description = description;
    this.pricePerMonth = +pricePerMonth;

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

    this.deletable = Boolean(deletable);
  }
}
