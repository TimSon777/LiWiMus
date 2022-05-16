import { Filter } from "./Filter";
import { Page } from "./Page";
import { Sorting } from "./Sorting";

export type FilterOptions<T> = {
  filters?: Filter<T>[];
  page?: Page;
  sorting?: Sorting<T>[];
};
