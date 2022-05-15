import {CommonEntity} from "../shared/commonEntity";
import {Column, Entity, JoinTable, ManyToMany, OneToMany} from "typeorm";
import {Track} from "../tracks/track.entity";
import {Artist} from "../artists/artist.entity";
import {Exclude} from "class-transformer";

@Entity("albums")
export class Album extends CommonEntity {
    @OneToMany(() => Track, track => track.album)
    tracks: Track[]
    
    @Column({ name: 'Title', length: 50 })
    title: string;

    @Column({ name: 'PublishedAt', type: 'date' })
    publishedAt: string;

    @Exclude()
    @Column({ name: 'CoverLocation' })
    coverLocation: string;

    @ManyToMany(() => Artist, artist => artist.albums)
    artists: Artist[];
}