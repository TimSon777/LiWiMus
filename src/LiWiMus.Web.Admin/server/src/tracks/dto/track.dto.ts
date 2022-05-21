import {IdDto} from "../../shared/dto/id.dto";
import {Genre} from "../../genres/genre.entity";
import {Column, JoinTable, ManyToMany, OneToMany} from "typeorm";
import {Artist} from "../../artists/artist.entity";
import {PlaylistTrack} from "../../playlistTracks/playlistTrack.entity";
import {Exclude, Expose} from "class-transformer";
import {Album} from "../../albums/album.entity";
import {GenreDto} from "../../genres/dto/genre.dto";
import {IsDateString, IsNotEmpty, IsNumber, IsString, MaxLength, ValidateNested} from "class-validator";
import {ApiProperty} from "@nestjs/swagger";
import {ArtistsDto} from "../../artists/dto/artists.dto";
import {AlbumDto} from "../../albums/dto/album.dto";

@Exclude()
export class TrackDto extends IdDto{
    @ApiProperty()
    @Expose()
    name: string;

    @ApiProperty()
    @Expose()
    genres: GenreDto[];

    @ApiProperty()
    @Expose()
    album: AlbumDto; 

    @ApiProperty()
    @Expose()
    publishedAt: Date;

    @ApiProperty()
    @Expose()
    artists: ArtistsDto[];

    @ApiProperty()
    @Expose()
    duration: number;

    @ApiProperty()
    @Expose()
    fileLocation: string;

    @ApiProperty()
    @Expose()
    createdAt: Date;

    @ApiProperty()
    @Expose()
    modifiedAt: Date;
}