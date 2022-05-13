import { Injectable } from '@nestjs/common';
import {User} from "./user.entity";
import {UpdateUserDto} from "./dto/update.user.dto";
import {DateSetterService} from "../shared/update.modifiedAt/set.date";

@Injectable()
export class UsersService {
    constructor(private readonly dateSetter: DateSetterService) {
    }
        async updateUser(dto: UpdateUserDto){
            let updatedUser = await User.findOne(dto.id);
            if(updatedUser) {
                updatedUser.modifiedAt = await this.dateSetter.setDate();
                await User.update({id: dto.id}, dto);

                if(dto.email && dto.userName){
                    await User.update({id: dto.id},
                        { normalizedEmail: dto.email.toUpperCase(),
                            normalizedUserName: dto.userName.toUpperCase()})
                }

                return User.findOne(dto.id)
            }
        }
    }