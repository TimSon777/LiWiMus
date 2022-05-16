import {Injectable} from "@nestjs/common";
import {MoreThan, LessThan, LessThanOrEqual, MoreThanOrEqual, Equal, Like, Not, In} from "typeorm"
import {Filter} from "../filter";

@Injectable()
export class FilterService {
    private static GetConditionalByOperator(operator : Operator, value : any) : any {
        if (operator[0] === '-') {
            let positiveOperator = operator.substring(1, operator.length) as Operator
            return Not(FilterService.GetConditionalByOperator(positiveOperator, value))
        }
        
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
            case "in": {
                return In(value)
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