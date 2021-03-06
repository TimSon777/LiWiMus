import {
    Body,
    Controller,
    Get,
    HttpException,
    HttpStatus, Param,
    Patch, Post,
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
import {ApiBearerAuth, ApiCreatedResponse, ApiOkResponse, ApiTags} from "@nestjs/swagger";
import {PaginatedData} from "../pagination/paginatied.data";
import {plainToInstance} from "class-transformer";

@Controller("users")
@ApiTags('users')
@ApiBearerAuth('swagger')
export class UsersController {
    constructor(private readonly filterOptionsService: FilterOptionsService, private readonly userService: UsersService){}

    @Get(':id')
    @ApiOkResponse( {type: UserDto })
    async getUserById(@Param('id') id : string)
        : Promise<UserDto> {
        let user = await User.findOne(+id);
        if(!user){
            throw new HttpException({
                message: "This user does nor exist"
            }, HttpStatus.UNPROCESSABLE_ENTITY);
        }
            
        return plainToInstance(UserDto, user);
    }
    
    @Get()
    @ApiOkResponse({ type: [PaginatedData] })
    async getUsers(@Query() options : FilterOptions) : Promise<PaginatedData<UserDto>> {
        let normalizedOptions = this.filterOptionsService.NormalizeOptions(options);
        let obj = this.filterOptionsService.GetFindOptionsObject(normalizedOptions);
        let data = await User.find(obj)
            .then(items => items.map(data => plainToInstance(UserDto, data)))
            .catch(err => {
                throw new HttpException(
                    {message: err.message}, 
                    HttpStatus.BAD_REQUEST)
            });
        let count = await User.count({where: obj.where});
        return new PaginatedData<UserDto>(data, normalizedOptions, count);
    }
    
    @Patch()
    @UsePipes(new ValidationPipe({skipMissingProperties: true}))
    @ApiCreatedResponse({ type: User })
    @UseInterceptors(new TransformInterceptor(UserDto))
    async updateUserPersonal(@Body() dto: UpdateUserDto) : Promise<UserDto>{
            return await this.userService.updateUser(dto);
    }

    @Post(':id/removeAvatar')
    @ApiCreatedResponse( {type: UserDto })
    async removeUserAvatar(@Param('id') id: string) {
        return await this.userService.removeAvatar(+id);
    }
}




