import {IdDto} from "../../shared/dto/id.dto";
import {Playlist} from "../../playlists/playlist.entity";
import {IsDefined, IsNotEmpty, IsNumber, IsPositive, ValidateNested} from "class-validator";
import {Type} from "class-transformer";
import {UserArtist} from "../../userArtist/userArtist.entity";

export class UpdateUserPlaylistsDto extends IdDto {
    @IsNumber()
    @IsPositive({each: true})
    playlistsId: number[];
} 