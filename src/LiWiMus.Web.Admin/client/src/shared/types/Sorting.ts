type Order = "ASC" | "DESC";

export type Sorting<T> = {
  columnName: keyof T;
  order: Order;
};
