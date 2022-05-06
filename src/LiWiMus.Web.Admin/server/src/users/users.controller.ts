import {Body, Controller, Get, HttpException, HttpStatus, Post, Query, UsePipes, ValidationPipe} from '@nestjs/common';
import {User} from "./user.entity";
import {FilterOptions} from "../filters/filter.options";
import {FilterOptionsService} from "../filters/services/filter.options.service";
import {UsersService} from "./users.service";
import {UpdateUserPersonalDto} from "./dto/updateUserPersonal.dto";
import {UpdateUserRoleDto} from "./dto/updateUserRole.dto";
import {UpdateUserSiteInformationDto} from "./dto/updateUserSiteInformation.dto";
import {UpdateUserPlaylistsDto} from "./dto/updateUserPlaylists.dto";
import {UpdateUserArtistDto} from "./dto/updateUserArtist.dto";
import {UserArtist} from "../userArtist/userArtist.entity";


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
    @UsePipes(new ValidationPipe({skipMissingProperties: true}))
    async updateUserPersonal(@Body() dto: UpdateUserPersonalDto){
            return await this.userService.updateUserPersonal(dto)
                .catch(err => {
                    throw new HttpException({
                        message: err.message
                    }, HttpStatus.BAD_REQUEST)
                });
        }

    @Post('updateUserRole')
    @UsePipes(new ValidationPipe({skipMissingProperties: true}))
    async updateUserRole(@Body() dto: UpdateUserRoleDto){
            return await this.userService.updateUserRole(dto)
                .catch(err => {
                    throw new HttpException({
                        message: err.message
                    }, HttpStatus.BAD_REQUEST)
                });
        }

    @Post('updateUserSiteInformation')
    @UsePipes(new ValidationPipe({skipMissingProperties: true}))
    async updateUserSiteInformation(@Body() dto: UpdateUserSiteInformationDto){
            return await this.userService.updateUserSiteInformation(dto)
                .catch(err => {
                    throw new HttpException({
                        message: err.message
                    }, HttpStatus.BAD_REQUEST)
                });
        }

    @Post('updateUserPlaylists')
    @UsePipes(new ValidationPipe({skipMissingProperties: true}))
    async updateUserPlaylists(@Body() dto: UpdateUserPlaylistsDto){
            return await this.userService.updateUserPlaylists(dto)
                .catch(err => {
                    throw new HttpException({
                        message: err.message
                    }, HttpStatus.BAD_REQUEST)
                });
        }

    @Post('updateUserArtist')
    @UsePipes(new ValidationPipe({skipMissingProperties: true}))
    async updateUserArtist(@Body() dto: UpdateUserArtistDto){

        return await UserArtist.create({
                artist: {id: 17,},
                user: {id: 13}
            })
            .save();
            //await UserArtist.save(userArtist);
            /*    return await this.userService.updateUserArtist(dto)
                    .catch(err => {
                        throw new HttpException({
                            message: err.message
                        }, HttpStatus.BAD_REQUEST)
                    });*/
        }
    }

//http://localhost:3001/api/users/getall?options[page][numberOfElementsOnPage]=3&options[page][pageNumber]=1&options[sorting][0][columnName]=id&options[sorting][0][order]=DESC