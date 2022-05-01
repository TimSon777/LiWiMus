import {Injectable} from "@nestjs/common";
import {Sorting} from "../sorting";

@Injectable()
export class SortService {
    public GetOrderObject(sorting: Sorting[]) : any {
        let orderObj = {};
        for(let i = 0; i < sorting.length; i++) {
            const sort = sorting[i];
            orderObj[sort.columnName] = sort.order;
        }
        return orderObj;
    }
}