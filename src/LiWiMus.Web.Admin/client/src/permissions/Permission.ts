import { addHours, parseISO } from "date-fns";

export class Permission {
  constructor(
    id: string,
    name: string,
    description: string,
    createdAt: string,
    modifiedAt: string
  ) {
    this.id = +id;
    this.name = name;
    this.description = description;

    this.createdAt = addHours(parseISO(createdAt), 3);
    this.modifiedAt = addHours(parseISO(modifiedAt), 3);
  }

  id: number;
  name: string;
  description: string;

  createdAt: Date;
  modifiedAt: Date;
}
