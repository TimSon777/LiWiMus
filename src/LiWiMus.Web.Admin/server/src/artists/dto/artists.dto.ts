import {IdDto} from "../../shared/dto/id.dto";
import {IsString, MaxLength} from "class-validator";
import {UserArtist} from "../../userArtist/userArtist.entity";
import {Album} from "../../albums/album.entity";
import {JoinTable, ManyToMany} from "typeorm";
import {Track} from "../../tracks/track.entity";
import {Exclude, Expose} from "class-transformer";
import {ApiProperty} from "@nestjs/swagger";
import {UserDto} from "../../users/dto/user.dto";
import {User} from "../../users/user.entity";
import {AlbumDto} from "../../albums/dto/album.dto";
import {TrackDto} from "../../tracks/dto/track.dto";


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
    albums: AlbumDto[];

    @ApiProperty()
    @Expose()
    photoLocation: string

    @ApiProperty()
    @Expose()
    createdAt: Date;

    @ApiProperty()
    @Expose()
    modifiedAt: Date;

    @ApiProperty()
    @Expose()
    users: UserDto[];
}