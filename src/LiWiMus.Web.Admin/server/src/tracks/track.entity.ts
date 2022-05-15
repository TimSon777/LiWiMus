import {CommonEntity} from '../shared/commonEntity';
import {Column, Entity, JoinColumn, JoinTable, ManyToMany, ManyToOne, OneToMany} from 'typeorm';
import {Album} from "../albums/album.entity";
import {Genre} from "../genres/genre.entity";
import {Artist} from "../artists/artist.entity";
import {PlaylistTrack} from "../playlistTracks/playlistTrack.entity";

@Entity('tracks')
export class Track extends CommonEntity {
    @ManyToOne(() => Album, album => album.tracks)
    @JoinColumn({ name: 'AlbumId', referencedColumnName: 'id' })
    album: Album

    @ManyToMany(() => Genre, genre => genre.tracks)
    @JoinTable({
        name: 'genretrack',
        joinColumn: { name: 'TracksId', referencedColumnName: 'id' },
        inverseJoinColumn: {name: 'GenresId', referencedColumnName: 'id' }
    })
    genres: Genre[];
    
    @Column({ name: 'Name', length: 50 })
    name: string;
    
    @Column({ name: 'PublishedAt', type: 'date' })
    publishedAt: Date;

    @Column({ name: 'FileLocation' })
    fileLocation: string;

    @ManyToMany(() => Artist, artist => artist.tracks)
    artists: Artist[];
    
    @OneToMany(() => PlaylistTrack, playlistTrack => playlistTrack.track)
    playlists: PlaylistTrack[];
}