import {IdDto} from "../../shared/dto/id.dto";
import {ApiProperty} from "@nestjs/swagger";
import {Exclude, Expose} from "class-transformer";
import {IsArray, IsInt} from "class-validator";

@Exclude()
export class UserArtistDto {
    @ApiProperty()
    @Expose()
    @IsArray()
    @IsInt({each: true})
    userIds: number[];
}