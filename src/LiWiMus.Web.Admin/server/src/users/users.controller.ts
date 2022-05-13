import {
    Body,
    Controller,
    Get,
    HttpException,
    HttpStatus,
    Post,
    Query,
    UseInterceptors,
    UsePipes,
    ValidationPipe
} from '@nestjs/common';
import {User} from "./user.entity";
import {FilterOptions} from "../filters/filter.options";
import {FilterOptionsService} from "../filters/services/filter.options.service";
import {UsersService} from "./users.service";
import {TransformInterceptor} from "../transformInterceptor/transform.interceptor";
import {UserDto} from "./dto/user.dto";
import {UpdateUserDto} from "./dto/update.user.dto";


@Controller("users")
export class UsersController {
    constructor(private readonly filterOptionsService: FilterOptionsService,
                private readonly userService: UsersService){}
    @Get('getall')
    @UseInterceptors(new TransformInterceptor(UserDto))
    async getUsers(@Query() options : FilterOptions)
        : Promise<User[]> {
        return User.find(
            this.filterOptionsService.GetFindOptionsObject(options, ['userArtists', 'transactions', 'playlists']))
            .catch(err => {
                throw new HttpException({
                    message: err.message
                }, HttpStatus.BAD_REQUEST)
            });
    }

    @Post('deleteUser')
    async deleteUser(@Body() id: number){
        let user = await User.findOne(id);
        await User.remove(user)
            .catch(err => {
            throw new HttpException({
                message: err.message
            }, HttpStatus.BAD_REQUEST)
        });
        return true;
    }
    
    @Post('updateUser')
    @UsePipes(new ValidationPipe({skipMissingProperties: true}))
    @UseInterceptors(new TransformInterceptor(UserDto))
    async updateUserPersonal(@Body() dto: UpdateUserDto){
            return await this.userService.updateUser(dto)
                .catch(err => {
                    throw new HttpException({
                        message: err.message
                    }, HttpStatus.BAD_REQUEST)
                });
        } 
    }

//http://localhost:3001/api/users/getall?options[page][numberOfElementsOnPage]=3&options[page][pageNumber]=1&options[sorting][0][columnName]=id&options[sorting][0][order]=DESC