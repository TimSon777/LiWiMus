import {Column, Entity, JoinTable, ManyToMany, OneToMany} from 'typeorm';
import { CommonEntity } from '../shared/commonEntity';
import {Album} from "../albums/album.entity";
import {Track} from "../tracks/track.entity";
import {UserArtist} from "../userArtist/userArtist.entity";

@Entity('Artists')
export class Artist extends CommonEntity {
    @Column({ name: 'Name', length: 50 })
    name: string

    @Column({ name: 'About', length: 500 })
    about: string
    
    @Column({ name: 'PhotoPath' })
    photoPath: string

    @OneToMany(() => UserArtist, ua => ua.artist)
    userArtists: UserArtist[]

    @ManyToMany(() => Album, album => album.artists)
    @JoinTable({ name: 'albumartist', joinColumn: { name: 'OwnersId' } })
    albums: Album[];

    @ManyToMany(() => Track, track => track.artists)
    @JoinTable({ name: 'artisttrack', joinColumn: { name: 'OwnersId' } })
    tracks: Track[];
}