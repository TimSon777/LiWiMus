import {Injectable} from "@nestjs/common";
import {Page} from "./page";

@Injectable()
export class PaginationService {
    public GetSkipObject(page: Page) : any {
        return (page.pageNumber - 1) * page.numberOfElementsOnPage
    }

    public GetTakeObject(page: Page) : any {
       return  page.numberOfElementsOnPage;
    }
}