import { Injectable } from '@nestjs/common';
import {UpdateUserPersonalDto} from "./dto/updateUserPersonal.dto";
import {User} from "./user.entity";
import {UpdateUserRoleDto} from "./dto/updateUserRole.dto";
import {IdDto} from "../shared/dto/id.dto";
import {UpdateUserSiteInformationDto} from "./dto/updateUserSiteInformation.dto";
import {UpdateUserPlaylistsDto} from "./dto/updateUserPlaylists.dto";
import {UpdateUserArtistDto} from "./dto/updateUserArtist.dto";

@Injectable()
export class UsersService {
        async updateUser(dto: any){
            await User.update({id: dto.id}, dto);
            return User.findOne(dto.id)
        }

        async updateUserPersonal(dto: UpdateUserPersonalDto){
            return this.updateUser(dto);
        }

        async updateUserRole(dto: UpdateUserRoleDto){
            return this.updateUser(dto);
        }

        async updateUserSiteInformation(dto: UpdateUserSiteInformationDto){
            return this.updateUser(dto);
        }

        async updateUserPlaylists(dto: UpdateUserPlaylistsDto){
            return this.updateUser(dto);
        }

        async updateUserArtist(dto: UpdateUserArtistDto){
            return this.updateUser(dto);
        }
    }