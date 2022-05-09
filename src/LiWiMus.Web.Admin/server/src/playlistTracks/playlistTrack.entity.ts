import {CommonEntity} from "../shared/commonEntity";
import {Column, Entity, JoinColumn, ManyToOne} from "typeorm";
import {Playlist} from "../playlists/playlist.entity";
import {Track} from "../tracks/track.entity";

@Entity('playlisttrack')
export class PlaylistTrack extends CommonEntity {
    @JoinColumn({ name: 'PlaylistId', referencedColumnName: 'id' })
    @ManyToOne(() => Playlist, playlist => playlist.tracks)
    playlist: Playlist;

    @JoinColumn({ name: 'TrackId', referencedColumnName: 'id' })
    @ManyToOne(() => Track, track => track.playlists)
    track: Track;
}