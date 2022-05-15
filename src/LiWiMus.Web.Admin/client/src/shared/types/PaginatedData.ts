export type PaginatedData<T> = {
  actualPage: number;
  itemsPerPage: number;
  totalItems: number;
  totalPages: number;
  data: T[];
  hasMore: boolean;
};
