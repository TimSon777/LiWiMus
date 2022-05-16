export type PaginatedData<T> = {
  actualPage: number;
  itemsPerPage: number;
  totalItems: number;
  totalPages: number;
  data: T[];
  hasMore: boolean;
};

export const DefaultPaginatedData = {
  actualPage: 0,
  totalPages: 0,
  totalItems: 0,
  itemsPerPage: 0,
  data: [],
  hasMore: true,
};
