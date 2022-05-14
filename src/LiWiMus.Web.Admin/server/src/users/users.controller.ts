import {
    Body,
    Controller,
    Get,
    HttpException,
    HttpStatus, Patch,
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
import {ApiCreatedResponse, ApiOkResponse, ApiTags} from "@nestjs/swagger";


@Controller("users")
@ApiTags('users')
export class UsersController {
    constructor(private readonly filterOptionsService: FilterOptionsService,
                private readonly userService: UsersService){}
    
    @Get('getall')
    @UseInterceptors(new TransformInterceptor(UserDto))
    @ApiOkResponse({ type: [User] })
    async getUsers(@Query() options : FilterOptions)
        : Promise<User[]> {
        return User.find(
            this.filterOptionsService.GetFindOptionsObject(options))
            .catch(err => {
                throw new HttpException({
                    message: err.message
                }, HttpStatus.BAD_REQUEST)
            });
    }
    
    @Patch('updateUser')
    @UsePipes(new ValidationPipe({skipMissingProperties: true}))
    @ApiCreatedResponse({ type: User })
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