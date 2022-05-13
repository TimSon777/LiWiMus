import {CommonEntity} from "../shared/commonEntity";
import {Column, Entity, ManyToMany, JoinTable} from "typeorm";
import {Track} from "../tracks/track.entity";
import {Exclude, Expose} from "class-transformer";
import {UseInterceptors} from "@nestjs/common";
import {TransformInterceptor} from "../transformInterceptor/transform.interceptor";
import {TrackDto} from "../tracks/dto/track.dto";
import {GenreDto} from "./dto/genre.dto";


@Entity('genres')
export class Genre extends CommonEntity {
    @Column({ name: 'Name', length: 50 })
    name: string;
    
    @ManyToMany(() => Track, track => track.genres)
    tracks: Track[];
}