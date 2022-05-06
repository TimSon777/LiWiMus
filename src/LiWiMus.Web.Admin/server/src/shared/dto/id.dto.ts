import {IsDefined} from "class-validator";

export class IdDto {
    @IsDefined()
    id: number;
} 