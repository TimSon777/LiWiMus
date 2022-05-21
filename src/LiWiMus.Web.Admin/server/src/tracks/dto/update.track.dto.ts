import {IdDto} from "../../shared/dto/id.dto";
import {Exclude, Expose} from "class-transformer";
import {ApiProperty} from "@nestjs/swagger";
import {GenreDto} from "../../genres/dto/genre.dto";
import {AlbumDto} from "../../albums/dto/album.dto";
import {ArtistsDto} from "../../artists/dto/artists.dto";
import {IsArray, IsDateString, IsInt, IsString, MaxLength} from "class-validator";

@Exclude()
export class UpdateTrackDto extends IdDto {
    @ApiProperty()
    @Expose()
    @IsString()
    @MaxLength(50)
    name: string;

    @ApiProperty()
    @Expose()
    duration: number;

    @ApiProperty()
    @Expose()
    @IsDateString()
    publishedAt: Date;

    @ApiProperty()
    @Expose()
    fileLocation: string;
}
