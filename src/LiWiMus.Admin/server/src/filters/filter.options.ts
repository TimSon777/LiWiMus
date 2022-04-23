import {Filter} from "./filter";
import {Page} from "../pagination/page";
import {Sorting} from "./sorting";

export class FilterOptions {
    filters: Filter[];
    page: Page;
    sorting: Sorting[];
}