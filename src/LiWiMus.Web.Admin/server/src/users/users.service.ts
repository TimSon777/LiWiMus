import {HttpException, HttpStatus, Injectable} from '@nestjs/common';
import {User} from "./user.entity";
import {UpdateUserDto} from "./dto/update.user.dto";
import {DateSetterService} from "../shared/setDate/set.date";
import {UserDto} from "./dto/user.dto";
import {plainToInstance} from "class-transformer";

@Injectable()
export class UsersService {
    constructor(private readonly dateSetter: DateSetterService) {}
        
    async updateUser(dto: UpdateUserDto) : Promise<UserDto> {
        if (await User.findOne(dto.id)) {
            let updatedUser = User.create(dto);
            updatedUser.modifiedAt = await this.dateSetter.setDate();
            await User.save(updatedUser);
            return plainToInstance(UserDto, User.findOne(dto.id));
        }
        else {
            throw new HttpException({
                message: "User does not exist."
            }, HttpStatus.UNPROCESSABLE_ENTITY);
        }
    }
    
    async removeAvatar(id: number) : Promise<UserDto> {
        if (await User.findOne(id)) {
            await User.update({id: id}, {avatarLocation: null});
            return plainToInstance(UserDto, User.findOne(id));
        }
        else {
            throw new HttpException({
                message: "User does not exist."
            }, HttpStatus.UNPROCESSABLE_ENTITY);
        }
    }
}