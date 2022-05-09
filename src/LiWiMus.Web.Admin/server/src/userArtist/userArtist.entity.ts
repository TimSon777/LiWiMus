import {CommonEntity} from "../shared/commonEntity";
import {Column, Entity, JoinColumn, ManyToOne} from "typeorm";
import {User} from "../users/user.entity";
import {Artist} from "../artists/artist.entity";

@Entity('userartist')
export class UserArtist extends CommonEntity {
    @ManyToOne(() => User, user => user.userArtists)
    @JoinColumn({ name: 'UserId', referencedColumnName: 'id' })
    user: User;

    @ManyToOne(() => Artist, user => user.userArtists)
    @JoinColumn({ name: 'ArtistId', referencedColumnName: 'id' })
    artist: Artist;
}