import {
    Body,
    Controller, Delete,
    Get,
    HttpException,
    HttpStatus, Patch,
    Post,
    Query,
    UseInterceptors,
    UsePipes,
    ValidationPipe
} from '@nestjs/common';
import {FilterOptionsService} from "../filters/services/filter.options.service";
import {FilterOptions} from "../filters/filter.options";
import {Artist} from "./artist.entity";
import {User} from "../users/user.entity";
import {Track} from "../tracks/track.entity";
import {UserArtist} from "../userArtist/userArtist.entity";
import {Album} from "../albums/album.entity";
import {TransformInterceptor} from "../transformInterceptor/transform.interceptor";
import {TrackDto} from "../tracks/dto/track.dto";
import {ArtistsDto} from "./dto/artists.dto";
import {ApiOkResponse, ApiTags} from "@nestjs/swagger";

@Controller('artists')
@ApiTags('artists')
export class ArtistsController {
    constructor(private readonly filterOptionsService: FilterOptionsService){}
    @Get('getall')
    @UseInterceptors(new TransformInterceptor(ArtistsDto))
    @ApiOkResponse({ type: [Artist] })
    async getArtists(@Query() options : FilterOptions)
        : Promise<Artist[]> {
        return Artist.find(
            this.filterOptionsService.GetFindOptionsObject(options))
            
            .catch(err => {
                throw new HttpException({
                    message: err.message
                }, HttpStatus.BAD_REQUEST)
            });
    }

    @Delete('deleteArtist')
    @UseInterceptors(new TransformInterceptor(ArtistsDto))
    async deleteUser(@Body() id: number){
        let artist = await Artist.findOne(id);
        await Artist.remove(artist)
            .catch(err => {
                throw new HttpException({
                    message: err.message
                }, HttpStatus.BAD_REQUEST)
            });
        return true;
    }

    @Patch('updateArtist')
    @UsePipes(new ValidationPipe({skipMissingProperties: true, whitelist: true}))
    async updateArtist(@Body() dto: any) : Promise<ArtistsDto> {
        await Artist.update({id: dto.id}, dto);
        return Artist.findOne(dto.id);
    }

   /* @Post('updateUserArtist')
    @UsePipes(new ValidationPipe({skipMissingProperties: true, whitelist: true}))
    @UseInterceptors(new TransformInterceptor(ArtistsDto))
    async updateUserArtist(@Body() dto: any)   {
        let artist = await Artist.findOne(dto.id);
        let users = await User.find({
            where: dto.userArtistsId.map((id) => ({id} as User))
        });

        let userArtists: UserArtist[] = [];
        let myDate = new Date();
        
        users.forEach(function (user)  {
            let userArtist = UserArtist.create({user: user, artist: artist, 
                createdAt: myDate, modifiedAt: myDate});
            userArtists.push(userArtist)
        })

         await UserArtist.save(userArtists)
            .catch(err => {
            throw new HttpException({
                message: err.message
            }, HttpStatus.BAD_REQUEST)
        });
        
    }

    @Post('updateArtistAlbumTracks')
    @UsePipes(new ValidationPipe({skipMissingProperties: true, whitelist: true}))
    @UseInterceptors(new TransformInterceptor(ArtistsDto))
    
    async updateArtistsAlbumTracks(@Body() dto: any)  {
        let tracks = await Track.find({
            where: dto.tracksId.map((id) => ({id} as Track))
        });

        let albums = await Album.find({
            where: dto.albumsId.map((id) => ({id} as Album))
        });
        
        
    }*/
}
//http://localhost:3001/api/artists/getall?options[page][numberOfElementsOnPage]=3&options[page][pageNumber]=1&options[sorting][0][columnName]=id&options[sorting][0][order]=DESC&options[filters][0][columnName]=name&options[filters][0][operator]=cnt&options[filters][0][value]=n