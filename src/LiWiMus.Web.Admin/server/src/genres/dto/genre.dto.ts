import {IdDto} from "../../shared/dto/id.dto";
import {Track} from "../../tracks/track.entity";
import {IsArray, IsPositive, IsString, MaxLength} from "class-validator";
import {Exclude, Expose, Type} from "class-transformer";
import {ApiProperty} from "@nestjs/swagger";
import {TrackDto} from "../../tracks/dto/track.dto";


@Exclude()
export class GenreDto extends IdDto {
    @ApiProperty()
    @MaxLength(50)
    @IsString()
    @Expose()
    name: string;

    @ApiProperty()
    @Expose()
    createdAt: Date;

    @ApiProperty()
    @Expose()
    modifiedAt: Date;

   /* @ApiProperty()
    @Expose()
    tracksAmount: number;*/
}