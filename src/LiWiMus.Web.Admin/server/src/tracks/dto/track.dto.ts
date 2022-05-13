import {IdDto} from "../../shared/dto/id.dto";
import {Genre} from "../../genres/genre.entity";
import {Column, JoinTable, ManyToMany, OneToMany} from "typeorm";
import {Artist} from "../../artists/artist.entity";
import {PlaylistTrack} from "../../playlistTracks/playlistTrack.entity";
import {Exclude, Expose} from "class-transformer";
import {Album} from "../../albums/album.entity";
import {GenreDto} from "../../genres/dto/genre.dto";
import {IsNotEmpty, ValidateNested} from "class-validator";

@Exclude()
export class TrackDto extends IdDto{
/*    @Expose()
    albumName: string;

    @Expose()
    albumId: number;

    @Expose()
    genre: string[];
    
    @Expose()
    genreId: number[];

    @Expose()
    publishedAt: Date;

    @Expose()
    artistsId: number[];

    @Expose()
    artistsName: string[];

    @Expose()
    playlistsId: number[];

    @Expose()
    playlistsName: string[];*/

    @Expose()
    name: string;
    
    @Expose()
    @ValidateNested()
    @IsNotEmpty()
    genres: Genre[];
    
    @Expose()
    album: Album;
    
    @Expose()
    publishedAt: Date;

    @Expose()
    pathToFile: string;

    @Expose()
    artists: Artist[];

    @Expose()
    playlists: PlaylistTrack[];
}