import {Injectable} from "@nestjs/common";
import {Sort} from "../sort";

@Injectable()
export class SortService {
    public GetOrderObject(sorts: Sort[]) : any {
        let orderObj = {};
        for(let i = 0; i < sorts.length; i++) {
            const sort = sorts[i];
            orderObj[sort.columnName] = sort.order;
        }
        return orderObj;
    }
}