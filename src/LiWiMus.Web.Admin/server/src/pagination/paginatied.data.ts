import {FilterOptions} from "../filters/filter.options";

export class PaginatedData<T> {
    constructor(data: T[], options: FilterOptions, totalItems: number) {
        this.data = data;
        this.actualPage = parseInt(options.page.pageNumber.toString());
        this.itemsPerPage = parseInt(options.page.numberOfElementsOnPage.toString());
        this.totalItems = totalItems;
        this.totalPages = Math.ceil( totalItems / this.itemsPerPage);
        this.hasMore = this.actualPage < this.totalPages
    }
    
    actualPage: number;
    itemsPerPage: number;
    totalItems: number;
    totalPages: number;
    data: T[];
    hasMore: boolean;
}