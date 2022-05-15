import {
    Body,
    Controller,
    Get,
    HttpException,
    HttpStatus,
    Patch,
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
import {PaginatedData} from "../pagination/paginatied.data";
import {plainToInstance} from "class-transformer";


@Controller("users")
@ApiTags('users')
export class UsersController {
    constructor(private readonly filterOptionsService: FilterOptionsService,
                private readonly userService: UsersService){}
    
    @Get('getList')
    //@UseInterceptors(new TransformInterceptor(UserDto))
    @ApiOkResponse({ type: [PaginatedData] })
    async getUsers(@Query() options : FilterOptions)
        : Promise<PaginatedData<UserDto>> {
        
        let normalizedOptions = this.filterOptionsService.NormalizeOptions(options);
        let obj = this.filterOptionsService.GetFindOptionsObject(normalizedOptions);


        let data = await User.find(obj)
            .then(items => items.map(data => plainToInstance(UserDto, data)))
            .catch(err => {
                throw new HttpException({
                    message: err.message
                }, HttpStatus.BAD_REQUEST)
            });
        
        let count = await User.count({where: obj.where});

        return new PaginatedData<UserDto>(data, normalizedOptions, count);
        
        //return await this.repo.find()
        //       .then(items => items.map(e=>plainToClass(ItemDTO, classToPlain(e), { excludeExtraneousValues: true })));
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