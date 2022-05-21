import {ApiProperty} from "@nestjs/swagger";
import {Exclude, Expose, Type} from "class-transformer";
import {IsArray, IsInt, IsNumber, IsNumberString} from "class-validator";

@Exclude()
export class TrackArtistDto {
     @ApiProperty()
     @Expose()
     @IsNumber()
     @Type(() => Number)
     artistId: number;
}