import {IdDto} from "../../shared/dto/id.dto";
import {UserArtist} from "../../userArtist/userArtist.entity";

export class UpdateUserArtistDto extends IdDto {
    userArtists: UserArtist[];
}