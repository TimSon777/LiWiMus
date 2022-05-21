import {ApiProperty} from "@nestjs/swagger";
import {Exclude, Expose} from "class-transformer";
import {IsArray, IsInt, IsNumberString} from "class-validator";

@Exclude()
export class TrackAlbumDto {
     @ApiProperty()
     @Expose()
     albumId: number[];

}