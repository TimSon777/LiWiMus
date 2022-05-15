import {Injectable} from "@nestjs/common";
import {Page} from "./page";
import {PaginatedData} from "./paginatied.data";
import {FilterOptionsService} from "../filters/services/filter.options.service";
import {FilterService} from "../filters/services/filter.service";
import {SortService} from "../filters/services/sort.service";
import {UserDto} from "../users/dto/user.dto";

@Injectable()
export class PaginationService {
    
    public GetSkipObject(page: Page) : any {
        return (page.pageNumber - 1) * page.numberOfElementsOnPage
    }

    public GetTakeObject(page: Page) : any {
       return  page.numberOfElementsOnPage;
    }
}