import {IdDto} from "../../shared/dto/id.dto";
import {Track} from "../../tracks/track.entity";
import {IsArray, IsPositive, IsString, MaxLength} from "class-validator";
import {Exclude, Expose, Type} from "class-transformer";


@Exclude()
export class GenreDto extends IdDto {
    @MaxLength(50)
    @IsString()
    @Expose()
    name: string;

    @Expose()
    tracks: Track[];

    @Expose()
    createdAt: Date;

    @Expose()
    modifiedAt: Date;
}