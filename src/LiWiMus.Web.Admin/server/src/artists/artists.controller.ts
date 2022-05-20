import {
    Body,
    Controller, Delete,
    Get,
    HttpException,
    HttpStatus, Param, Patch,
    Post,
    Query, Res,
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
import {plainToInstance} from "class-transformer";
import {PaginatedData} from "../pagination/paginatied.data";
import {CreateArtistDto} from "./dto/create.artist.dto";
import {ArtistsService} from "./artists.service";
import {UserDto} from "../users/dto/user.dto";
import {UserArtistDto} from "./dto/user.artist.dto";

@Controller('artists')
@ApiTags('artists')
export class ArtistsController {
    constructor(private readonly filterOptionsService: FilterOptionsService, 
                private readonly artistsService: ArtistsService){}

    @Post()
    @ApiOkResponse({ type: [ArtistsDto] })
    async createArtist(@Body() dto: CreateArtistDto) : Promise<ArtistsDto> {
        return await this.artistsService.createArtist(dto);
    }

    @Get(':id')
    @ApiOkResponse({ type: ArtistsDto })
    async getArtistById(@Param('id') id : string) : Promise<ArtistsDto> {
        let artist = await Artist.findOne(+id, {relations: ['albums']})
            .catch(err => {
                throw new HttpException({
                    message: err.message
                }, HttpStatus.BAD_REQUEST)
            });
        
       return plainToInstance(ArtistsDto, artist);
    }


    @Get()
   // @UseInterceptors(new TransformInterceptor(ArtistsDto))
    @ApiOkResponse({ type: [ArtistsDto] })
    async getArtists(@Query() options : FilterOptions)
        : Promise<PaginatedData<ArtistsDto>>
    {
        let normalizedOptions = this.filterOptionsService.NormalizeOptions(options);
        let obj = this.filterOptionsService.GetFindOptionsObject(options, ['albums']);

        let data = await Artist.find(obj)
            .then(items => items.map(data => plainToInstance(ArtistsDto, data)))
            
            
            .catch(err => {
                throw new HttpException({
                    message: err.message
                }, HttpStatus.BAD_REQUEST)
            });

        let count = await Artist.count({where: obj.where});
        return new PaginatedData<ArtistsDto>(data, normalizedOptions, count);
    }

    @Get(":id/users")
    @ApiOkResponse({ type: [UserDto] })
    async getArtistsUsers(@Param('id') id : string)
        : Promise<UserDto[]>
    {
        let artist = await Artist.findOne(+id);
        let users = UserArtist.find({where: {artist: artist}, relations: ['user', 'artist']})
            .then(i => i.map((u) => (u.user)))
            .then(u => u.map(data => plainToInstance(UserDto, data)));
        return users;
    }

    @Post(":id/users")
    @ApiOkResponse({ type: [ArtistsDto] }) 
    async addUsers(@Param('id') id : string, @Body() dto: UserArtistDto) {
        return  await this.artistsService.addUsers(+id, dto);
    }

    @Delete(":id/users")
    @ApiOkResponse({ type: [ArtistsDto] })
    async deleteUsers(@Param('id') id : string, @Body() dto: UserArtistDto) {
        return await this.artistsService.deleteUsers(+id, dto);
    }
    
   /* @Delete()
    @UseInterceptors(new TransformInterceptor(ArtistsDto))
    async deleteArtist(@Body() id: number){
        let artist = await Artist.findOne(id);
        await Artist.remove(artist)
            .catch(err => {
                throw new HttpException({
                    message: err.message
                }, HttpStatus.BAD_REQUEST)
            });
        return true;
    }*/
    


   /* @Patch()
    @UsePipes(new ValidationPipe({skipMissingProperties: true, whitelist: true}))
    async updateArtist(@Body() dto: any) : Promise<ArtistsDto> {
        await Artist.update({id: dto.id}, dto);
        return Artist.findOne(dto.id);
    }*/

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