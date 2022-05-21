import {Exclude, Expose} from "class-transformer";
import {IsArray, IsInt, IsNumber, IsNumberString} from "class-validator";
import {ApiProperty} from "@nestjs/swagger";

@Exclude()
export class TrackGenreDto {
     @ApiProperty()
     @Expose()
     @IsArray()
     genresId: number[];
}