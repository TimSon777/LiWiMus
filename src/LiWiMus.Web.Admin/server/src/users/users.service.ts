import { Injectable } from '@nestjs/common';
import {User} from "./user.entity";
import {UpdateUserDto} from "./dto/update.user.dto";

@Injectable()
export class UsersService {
        async updateUser(dto: UpdateUserDto){
           await User.update({id: dto.id}, dto);
           
           if(dto.email && dto.userName){
               await User.update({id: dto.id},
                   { normalizedEmail: dto.email.toUpperCase(), 
                              normalizedUserName: dto.userName.toUpperCase()})
           }
           
           return User.findOne(dto.id)
        }
    }