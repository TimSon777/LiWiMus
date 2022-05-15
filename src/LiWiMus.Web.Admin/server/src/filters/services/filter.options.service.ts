import {Injectable} from "@nestjs/common";
import {FilterOptions} from "../filter.options";
import {FilterService} from "./filter.service";
import {SortService} from "./sort.service";
import {PaginationService} from "../../pagination/pagination.service";


@Injectable()
export class FilterOptionsService {
    constructor(private readonly filterService: FilterService,
                private readonly sortService: SortService,
                private readonly  paginationService: PaginationService) {
    }
    
    public NormalizeOptions(options : FilterOptions) : FilterOptions {
        
        if (!options) {
            options = new FilterOptions();
        }
        
        if (!options.filters) {
            options.filters = [];
        }
        
        if (!options.page) {
            options.page = {numberOfElementsOnPage: 10, pageNumber: 1};
        }
        
        if (!options.sorting) {
            options.sorting = [{columnName: "id", order: "ASC"}];
        }
        
        return options;
    }
    
    public GetFindOptionsObject(normalizeOptions : FilterOptions, includes : string[] = null) : any {
        
        let optionsObj = {};
        
        if (includes) {
            optionsObj['relations'] = includes;
        }
        
        optionsObj['where'] = this.filterService.GetWhereObject(normalizeOptions.filters);
        
        optionsObj['skip'] = this.paginationService.GetSkipObject(normalizeOptions.page);
        
        optionsObj['take'] = this.paginationService.GetTakeObject(normalizeOptions.page);
        
        optionsObj['order'] = this.sortService.GetOrderObject(normalizeOptions.sorting);
        
        return optionsObj;
    }
}