type Operator =
  | "eq"
  | "gt"
  | "ls"
  | "lse"
  | "gte"
  | "cnt"
  | "sw"
  | "in"
  | "-eq"
  | "-gt"
  | "-ls"
  | "-lse"
  | "-gte"
  | "-cnt"
  | "-sw"
  | "-in";

export type Filter<T> = {
  columnName: keyof T;
  operator: Operator;
  value: any;
};
