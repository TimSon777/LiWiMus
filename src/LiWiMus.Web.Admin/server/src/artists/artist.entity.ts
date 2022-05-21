import {Column, Entity, JoinTable, ManyToMany, OneToMany} from 'typeorm';
import { CommonEntity } from '../shared/commonEntity';
import {Album} from "../albums/album.entity";
import {Track} from "../tracks/track.entity";
import {UserArtist} from "../userArtist/userArtist.entity";
import {Exclude} from "class-transformer";

@Entity('artists')
export class Artist extends CommonEntity {
    @Column({ name: 'Name', length: 50 })
    name: string

    @Column({ name: 'About', length: 500 })
    about: string
    
    @Column({ name: 'PhotoLocation' })
    photoLocation: string

    @OneToMany(() => UserArtist, ua => ua.artist)
    userArtists: UserArtist[]

    @ManyToMany(() => Album, album => album.artists)
    @JoinTable({ 
        name: 'albumartist', 
        joinColumn: { name: 'OwnersId', referencedColumnName: 'id' },
        inverseJoinColumn: {name: 'AlbumsId', referencedColumnName: 'id' }
    })
    albums: Album[];

    @ManyToMany(() => Track, track => track.artists)
    @JoinTable({ 
        name: 'artisttrack', 
        joinColumn: { name: 'OwnersId', referencedColumnName: 'id' },
        inverseJoinColumn: {name: 'TracksId', referencedColumnName: 'id' }
    })
    tracks: Track[];
}