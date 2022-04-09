import {Column, Entity, JoinTable, ManyToMany, OneToMany, OneToOne} from 'typeorm';
import { CommonEntity } from '../shared/commonEntity';
import {User} from "../users/user.entity";
import {Album} from "../albums/album.entity";
import {Track} from "../tracks/track.entity";

@Entity('Artists')
export class Artist extends CommonEntity {
    @Column({ name: 'Name', length: 50 })
    name: string

    @Column({ name: 'About', length: 500 })
    about: string
    
    @Column({ name: 'PhotoPath' })
    photoPath: string

    @OneToOne(() => User, (user) => user.artist)
    user: User

    @ManyToMany(() => Album, album => album.artists)
    @JoinTable({ name: 'albumartist', joinColumn: { name: 'OwnersId' } })
    albums: Album[];

    @ManyToMany(() => Track, track => track.artists)
    @JoinTable({ name: 'artisttrack', joinColumn: { name: 'OwnersId' } })
    tracks: Track[];
}