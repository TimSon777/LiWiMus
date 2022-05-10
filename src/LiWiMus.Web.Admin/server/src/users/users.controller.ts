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
import {Artist} from "../artists/artist.entity";
import {Track} from "../tracks/track.entity";


@Controller("users")
export class UsersController {
    constructor(private readonly filterOptionsService: FilterOptionsService,
                private readonly userService: UsersService){}
    @Get('getall')
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

        let user = await User.findOne(dto.id);
        let artists = await Artist.find({
            where: dto.artistsId.map((id) => ({id} as Artist))
        });
        let userArtists: UserArtist[] = [];
        
        artists.forEach(artist => {
            let userArtist = UserArtist.create({user: user, artist: artist});
            userArtists.push(userArtist)
        })
        
        await UserArtist.save(userArtists);
        
        let updatedUser = User.create({id: dto.id, userArtists: userArtists});
        
        return await User.save(updatedUser)
              .catch(err => {
              throw new HttpException({
                   message: err.message
                   }, HttpStatus.BAD_REQUEST)
              });
        }
    }

//http://localhost:3001/api/users/getall?options[page][numberOfElementsOnPage]=3&options[page][pageNumber]=1&options[sorting][0][columnName]=id&options[sorting][0][order]=DESC