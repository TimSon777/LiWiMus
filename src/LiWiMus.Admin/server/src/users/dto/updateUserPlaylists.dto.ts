import {IdDto} from "../../shared/dto/id.dto";
import {Playlist} from "../../playlists/playlist.entity";

export class UpdateUserPlaylistsDto extends IdDto {
    playlists: Playlist[];
}