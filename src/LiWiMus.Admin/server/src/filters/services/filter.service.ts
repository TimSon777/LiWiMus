import {Injectable} from "@nestjs/common";
import { MoreThan, LessThan } from "typeorm"
import {Filter} from "../filter";

@Injectable()
export class FilterService {
    //operatoro preobrazovatel'
    //
    private static GetConditionalByOperator(operator : Operator, value : any) : any {
        switch(operator) {
            case "eq": {
                return value;
            }
            case "gt": {
                return MoreThan(value);
            }
            case "ls": {
                return LessThan(value);
            }
            //lse, gte, contains, startwith
            default: {
                return null;
            }
        }
    }
    
    public GetWhereObject(filters : Filter[]) : any {
        let whereObj = {};
        for(let i = 0; i < filters.length; i++) {
           const filter = filters[i];
           const conditional = FilterService.GetConditionalByOperator(filter.operator, filter.value);
           if (conditional) {
               whereObj[filter.columnName] = conditional;
           }
        }
        
        return whereObj;
    }
}