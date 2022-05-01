import {CommonEntity} from "../shared/commonEntity";
import {Column, Entity, JoinTable, ManyToMany, OneToMany} from "typeorm";
import {Track} from "../tracks/track.entity";
import {Artist} from "../artists/artist.entity";

@Entity("albums")
export class Album extends CommonEntity {
    @OneToMany(() => Track, track => track.album)
    tracks: Track[]
    
    @Column({ name: 'Title', length: 50 })
    title: string;

    @Column({ name: 'PublishedAt', type: 'date' })
    publishedAt: string;

    @Column({ name: 'CoverPath' })
    coverPath: string;

    @ManyToMany(() => Artist, artist => artist.albums)
    @JoinTable({ name: 'albumartist', joinColumn: { name: 'AlbumsId' } })
    artists: Artist[];
}