import {CommonEntity} from "../shared/commonEntity";
import {Column, Entity, ManyToOne} from "typeorm";
import {User} from "../users/user.entity";
import {Artist} from "../artists/artist.entity";

@Entity('userartist')
export class UserArtist extends CommonEntity {
    @ManyToOne(() => User, user => user.userArtists)
    @Column({ name: 'UserId', type: 'int' })
    user: User;

    @ManyToOne(() => Artist, user => user.userArtists)
    @Column({ name: 'ArtistId', type: 'int' })
    artist: Artist;
}