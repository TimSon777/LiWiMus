export type PaginatedDate<T> = {
  actualPage: number;
  itemsPerPage: number;
  totalItems: number;
  totalPages: number;
  data: T[];
};
