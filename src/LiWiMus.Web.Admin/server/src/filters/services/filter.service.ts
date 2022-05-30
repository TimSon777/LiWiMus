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

    private createRecObj(propArr, resultingObj, index: number = 0) {
        for (let j = index, len1 = propArr.length; j < len1; j += 1) {
            let prop = propArr[j];
            if (!resultingObj[prop]) {
                resultingObj[prop] = {};
            }
            if (propArr[j + 1] === propArr[propArr.length-1]) {
                resultingObj[prop] = propArr[j + 1];
                j += 1;
            } else {
                this.createRecObj(propArr, resultingObj[prop], j + 1);
                j = len1;
            }
        }
    }
    
    public GetWhereObject(filters : Filter[]) : any {
        let whereObj = {};
        for(let i = 0; i < filters.length; i++) {
           const filter = filters[i];
           const conditional = FilterService.GetConditionalByOperator(filter.operator, filter.value);
           if (conditional) {
               let recursiveColumnName : any[] = filter.columnName.split('.').concat(conditional);
               this.createRecObj(recursiveColumnName, whereObj);
           }
        }
        
        return whereObj;
    }
}