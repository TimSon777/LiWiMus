import {FilterOptions} from "../filters/filter.options";

export class PaginatedData<T> {
    constructor(data: T[], options: FilterOptions, totalItems: number) {
        this.data = data;
        this.actualPage = options.page.pageNumber;
        this.itemsPerPage = options.page.numberOfElementsOnPage;
        this.totalItems = totalItems;
        this.totalPages = Math.floor( totalItems / this.itemsPerPage);
        this.hasMore = this.actualPage < this.totalPages
    }
    
    actualPage: number;
    itemsPerPage: number;
    totalItems: number;
    totalPages: number;
    data: T[];
    hasMore: boolean;
}