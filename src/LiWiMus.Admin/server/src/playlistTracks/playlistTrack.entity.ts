import {CommonEntity} from "../shared/commonEntity";
import {Column, Entity, ManyToOne} from "typeorm";
import {Playlist} from "../playlists/playlist.entity";
import {Track} from "../tracks/track.entity";

@Entity('playlisttrack')
export class PlaylistTrack extends CommonEntity {
    @Column({ name: 'PlaylistId', type: 'int' })
    @ManyToOne(() => Playlist, playlist => playlist.tracks)
    playlist: Playlist;

    @Column({ name: 'TrackId', type: 'int' })
    @ManyToOne(() => Track, track => track.playlists)
    track: Track;
}