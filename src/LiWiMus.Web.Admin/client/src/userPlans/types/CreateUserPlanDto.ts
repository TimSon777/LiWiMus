export type CreateUserPlanDto = {
  userId: string | number;
  planId: string | number;
  start: Date | null;
  end: Date;
};
