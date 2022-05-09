import {IdDto} from "../../shared/dto/id.dto";
import {Genre} from "../../genres/genre.entity";
import {Column, JoinTable, ManyToMany, OneToMany} from "typeorm";
import {Artist} from "../../artists/artist.entity";
import {PlaylistTrack} from "../../playlistTracks/playlistTrack.entity";

export class TrackDto extends IdDto{
    albumName: string;
    albumId: number;
    genre: string[];
    genreId: number[];
    publishedAt: Date;
    artistsId: number[];
    artistsName: string[];
    playlists: PlaylistTrack[];
}