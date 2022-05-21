import {ApiProperty} from "@nestjs/swagger";
import {Exclude, Expose, Type} from "class-transformer";
import {IsArray, IsInt, IsNumber, IsNumberString} from "class-validator";

@Exclude()
export class TrackAlbumDto {
     @ApiProperty()
     @Expose()
     @IsNumber()
     @Type(() => Number)
     albumId:number;
}