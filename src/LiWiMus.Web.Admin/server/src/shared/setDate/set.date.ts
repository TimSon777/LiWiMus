import {CommonEntity} from "../commonEntity";

export class DateSetterService {
    async setDate() : Promise<Date>{
        let date = new Date().toUTCString();
           // .toISOString().slice(0, 19).replace('T', ' ');
        return date as any;
    }
}