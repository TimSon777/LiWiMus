import {FilterOptionsService} from "../filters/services/filter.options.service";

export class PaginatedData<T> {
    actualPage: number;
    itemsPerPage: number;
    totalItems: number;
    totalPages: number;
    data: T[];
    hasMore: boolean;
}