import {IdDto} from "../../shared/dto/id.dto";
import {IsString, MaxLength} from "class-validator";
import {UserArtist} from "../../userArtist/userArtist.entity";
import {Album} from "../../albums/album.entity";
import {JoinTable, ManyToMany} from "typeorm";
import {Track} from "../../tracks/track.entity";
import {Exclude, Expose} from "class-transformer";
import {ApiProperty} from "@nestjs/swagger";


@Exclude()
export class ArtistsDto extends IdDto {
    @ApiProperty()
    @Expose()
    @IsString()
    @MaxLength(50)
    name: string;

    @ApiProperty()
    @Expose()
    @IsString()
    @MaxLength(500)
    about: string;

    @ApiProperty()
    @Expose()
    userArtists: UserArtist[]

    @ApiProperty()
    @Expose()
    albums: Album[];

    @ApiProperty()
    @Expose()
    tracks: Track[];
}