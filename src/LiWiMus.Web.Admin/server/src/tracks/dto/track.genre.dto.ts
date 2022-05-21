import {Exclude, Expose, Type} from "class-transformer";
import {IsArray, IsInt, IsNumber, IsNumberString} from "class-validator";
import {ApiProperty} from "@nestjs/swagger";

@Exclude()
export class TrackGenreDto {
     @ApiProperty()
     @Expose()
     @IsNumber()
     @Type(() => Number)
     genreId: number;
}