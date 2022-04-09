import {CommonEntity} from "../shared/commonEntity";
import {Column, Entity, ManyToMany, ManyToOne, OneToMany} from "typeorm";
import {User} from "../users/user.entity";
import {Track} from "../tracks/track.entity";
import {PlaylistTrack} from "../playlistTracks/playlistTrack.entity";

@Entity('playlists')
export class Playlist extends CommonEntity {
    @ManyToOne(() => User, user => user.playlists)
    @Column({ name: 'OwnerId', type: 'int' })
    owner: User;

    @Column({ name: 'Name', length: 50 })
    name: string;
    
    @Column({ name: 'IsPublic' })
    isPublic: boolean;
    
    @Column({ name: 'PhotoPath' })
    photoPath: string;

    @OneToMany(() => PlaylistTrack, playlistTrack => playlistTrack.playlist)
    tracks: PlaylistTrack[];
}