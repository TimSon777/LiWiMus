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
    
    public GetFindOptionsObject(options : FilterOptions, includes : string[] = null) : any {
        let optionsObj = {};
        
        if (includes) {
            optionsObj['relations'] = includes;
        }
        
        optionsObj['where'] = this.filterService
            .GetWhereObject(FilterOptionsService.DefineObjectProperty(options.filters, []));
        
        optionsObj['skip'] = this.paginationService
            .GetSkipObject(FilterOptionsService.DefineObjectProperty(options.page, 
                {numberOfElementsOnPage: 10, pageNumber: 1}));
        
        optionsObj['take'] = this.paginationService
            .GetTakeObject(FilterOptionsService.DefineObjectProperty(options.page,
            {numberOfElementsOnPage: 10, pageNumber: 1}));
        
        optionsObj['order'] = this.sortService
            .GetOrderObject(FilterOptionsService.DefineObjectProperty(options.sorting, 
                [{columnName: "id", order: "ASC"}]));
        
        return optionsObj;
    }
    
    private static DefineObjectProperty(property : any, defaultProperty : any) : any {
        return property 
            ? property 
            : defaultProperty;
    }
}