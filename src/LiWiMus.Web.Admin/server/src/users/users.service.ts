import { Injectable } from '@nestjs/common';
import {UpdateUserPersonalDto} from "./dto/updateUserPersonal.dto";
import {User} from "./user.entity";

@Injectable()
export class UsersService {
    async updateUserPersonal(dto: UpdateUserPersonalDto){
        await User.update({id: dto.id}, dto);
        return User.findOne(dto.id)
    }
}

