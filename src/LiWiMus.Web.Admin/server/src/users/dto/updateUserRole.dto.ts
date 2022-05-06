import {IdDto} from "../../shared/dto/id.dto";
import {UserRole} from "../../userRoles/userRoles.entity";
import {IsDefined, IsNotEmpty, IsNumber, IsPositive, ValidateNested} from "class-validator";
import {Type} from "class-transformer";
import {Playlist} from "../../playlists/playlist.entity";

export class UpdateUserRoleDto extends IdDto {
    @IsNumber()
    @IsPositive({each: true})
    userRolesId: number[];
}