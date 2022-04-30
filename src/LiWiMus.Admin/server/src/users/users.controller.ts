import {Body, Controller, Get, HttpException, HttpStatus, Post, Query} from '@nestjs/common';
import {User} from "./user.entity";
import {FilterOptions} from "../filters/filter.options";
import {FilterOptionsService} from "../filters/services/filter.options.service";
import {UsersService} from "./users.service";
import {UpdateUserPersonalDto} from "./dto/updateUserPersonal.dto";


@Controller("users")
export class UsersController {
    constructor(private readonly filterOptionsService: FilterOptionsService, 
                private readonly userService: UsersService){}
    @Get('getall')
    async getUsers(@Query('options') options : FilterOptions)
        : Promise<User[]> {
            return User.find(
                this.filterOptionsService.GetFindOptionsObject(options))
                .catch(err => {
                throw new HttpException({
                    message: err.message
                }, HttpStatus.BAD_REQUEST)
            });
    }
    
    @Post('updateUserPersonal')
    async updateUser(@Body() dto: UpdateUserPersonalDto){
        return await this.userService.updateUserPersonal(dto)
            .catch(err => {
                throw new HttpException({
                    message: err.message
                }, HttpStatus.BAD_REQUEST)
            });
    }
}

//http://localhost:3001/api/users/getall?options[page][numberOfElementsOnPage]=3&options[page][pageNumber]=1&options[sorting][0][columnName]=id&options[sorting][0][order]=DESC

