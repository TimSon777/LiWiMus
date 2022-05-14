import {IsDefined, IsInt, IsPositive} from "class-validator";
import {Exclude, Expose} from "class-transformer";
import {ApiProperty} from "@nestjs/swagger";

@Exclude()
export class IdDto {
    @ApiProperty()
    @Expose()
    @IsDefined()
    @IsPositive()
    @IsInt()
    id: number;
} 