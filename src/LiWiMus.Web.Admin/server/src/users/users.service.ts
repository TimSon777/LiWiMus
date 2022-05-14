import { Injectable } from '@nestjs/common';
import {User} from "./user.entity";
import {UpdateUserDto} from "./dto/update.user.dto";
import {DateSetterService} from "../shared/setDate/set.date";
import {UserRole} from "../userRoles/userRoles.entity";
import {Role} from "../roles/role.entity";

@Injectable()
export class UsersService {
    constructor(private readonly dateSetter: DateSetterService) {}
        
    async updateUser(dto: UpdateUserDto){
        if (await User.findOne(dto.id)) {
            let updatedUser = User.create(dto);
            updatedUser.modifiedAt = await this.dateSetter.setDate();
            await User.save(updatedUser);

            if(dto.email){
                await User.update({id: dto.id},
                    { normalizedEmail: dto.email.toUpperCase()})
            }
            
            if(dto.userName){
                await User.update({id: dto.id},
                    {normalizedUserName: dto.userName.toUpperCase()})
            }
            
            return User.findOne(dto.id)
        }
    }
}