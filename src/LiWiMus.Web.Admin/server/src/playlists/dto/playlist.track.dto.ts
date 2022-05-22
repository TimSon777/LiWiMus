import {IdDto} from "../../shared/dto/id.dto";
import {ApiProperty} from "@nestjs/swagger";
import {Expose, Type} from "class-transformer";
import {IsArray, IsInt, IsNotEmpty} from "class-validator";

export class PlaylistTrackDto {
    @ApiProperty()
    @Expose()
    @IsNotEmpty()
    @IsArray()
    @Type(() => Number)
    @IsInt({each: true})
    tracks: number[];
}