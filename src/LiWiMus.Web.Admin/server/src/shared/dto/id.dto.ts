import {IsDefined} from "class-validator";
import {Exclude, Expose} from "class-transformer";

@Exclude()
export class IdDto {
    @Expose()
    @IsDefined()
    id: number;
} 