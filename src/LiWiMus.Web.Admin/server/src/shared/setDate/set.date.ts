import {CommonEntity} from "../commonEntity";

export class DateSetterService {
    async setDate() : Promise<Date>{
        let date = new Date().toUTCString();
        return date as any;
    }
}