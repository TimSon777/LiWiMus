import {Injectable} from "@nestjs/common";
import {MoreThan, LessThan, LessThanOrEqual, MoreThanOrEqual, Equal, Like} from "typeorm"
import {Filter} from "../filter";

@Injectable()
export class FilterService {
    private static GetConditionalByOperator(operator : Operator, value : any) : any {
        switch(operator) {
            case "eq": {
                return Equal(value);
            }
            case "gt": {
                return MoreThan(value);
            }
            case "ls": {
                return LessThan(value);
            }
            case "lse": {
                return LessThanOrEqual(value);
            }
            case "gte": {
                return MoreThanOrEqual(value);
            }
            case "cnt": {
                return Like(`%${value}%`)
            }
            case "sw": {
                return Like(`${value}%`)
            }
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