import {Artist} from "../../artists/artist.entity";
import {UserRole} from "../../userRoles/userRoles.entity";
import {Playlist} from "../../playlists/playlist.entity";
import {Transaction} from "../../transactions/transaction.entity";
import {ExternalLogin} from "../../externalLogins/externalLogin.entity";
import {UserArtist} from "../../userArtist/userArtist.entity";
import {IdDto} from "../../shared/dto/id.dto";

export class UpdateUserPersonalDto extends IdDto {
 firstName: string;
 secondName: string;
 patronymic: string;
 gender: "Male" | "Female";
 birthDate: Date;
}