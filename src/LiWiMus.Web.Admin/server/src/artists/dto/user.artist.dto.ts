import {IdDto} from "../../shared/dto/id.dto";
import {ApiProperty} from "@nestjs/swagger";
import {Exclude, Expose, Type} from "class-transformer";
import {IsArray, IsInt, IsNumber, IsNumberString} from "class-validator";

@Exclude()
export class UserArtistDto {
    @ApiProperty()
    @Expose()
    @IsArray()
    userIds: number[];
}