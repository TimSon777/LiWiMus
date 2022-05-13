import {IdDto} from "../../shared/dto/id.dto";
import {IsString, MaxLength} from "class-validator";
import {UserArtist} from "../../userArtist/userArtist.entity";
import {Album} from "../../albums/album.entity";
import {JoinTable, ManyToMany} from "typeorm";
import {Track} from "../../tracks/track.entity";
import {Exclude, Expose} from "class-transformer";


@Exclude()
export class ArtistsDto extends IdDto {
    @Expose()
    @IsString()
    @MaxLength(50)
    name: string;

    @Expose()
    @IsString()
    @MaxLength(500)
    about: string;

    @Expose()
    userArtists: UserArtist[]

    @Expose()
    albums: Album[];

    @Expose()
    tracks: Track[];
}