import {CommonEntity} from "../shared/commonEntity";
import {Column, Entity, ManyToMany, JoinTable} from "typeorm";
import {Track} from "../tracks/track.entity";

@Entity('genres')
export class Genre extends CommonEntity {
    @Column({ name: 'Name', length: 50 })
    name: string;
    
    @ManyToMany(() => Track, track => track.genres)
    tracks: Track[];
}