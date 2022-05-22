import {CommonEntity} from "../shared/commonEntity";
import {Column, Entity, JoinColumn, ManyToMany, ManyToOne, OneToMany} from "typeorm";
import {User} from "../users/user.entity";
import {Track} from "../tracks/track.entity";
import {PlaylistTrack} from "../playlistTracks/playlistTrack.entity";

@Entity('playlists')
export class Playlist extends CommonEntity {
    @ManyToOne(() => User, user => user.playlists)
    @JoinColumn({ name: 'OwnerId', referencedColumnName: 'id' })
    owner: User;

    @Column({ name: 'Name', length: 50 })
    name: string;
    
    @Column({ name: 'IsPublic' })
    isPublic: boolean;
    
    @Column({ name: 'PhotoLocation' })
    photoLocation: string;

    @OneToMany(() => PlaylistTrack, playlistTrack => playlistTrack.playlist)
    tracks: PlaylistTrack[];
}