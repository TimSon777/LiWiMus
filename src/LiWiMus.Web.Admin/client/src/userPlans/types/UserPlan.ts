import { parseISO } from "date-fns";

export class UserPlan {
  constructor(
    id: string,
    planName: string,
    planDescription: string,
    planId: string,
    userName: string,
    userId: string,
    start: string,
    end: string,
    createdAt: string,
    modifiedAt: string,
    updatable: string
  ) {
    this.id = +id;
    this.planName = planName;
    this.planDescription = planDescription;
    this.planId = +planId;
    this.userName = userName;
    this.userId = +userId;
    this.start = parseISO(start);
    this.end = parseISO(end);
    this.createdAt = parseISO(createdAt);
    this.modifiedAt = parseISO(modifiedAt);
    this.updatable = Boolean(updatable);
  }

  id: number;

  planName: string;
  planDescription: string;
  planId: number;

  userName: string;
  userId: number;

  start: Date;
  end: Date;

  createdAt: Date;
  modifiedAt: Date;

  updatable: boolean;
}
