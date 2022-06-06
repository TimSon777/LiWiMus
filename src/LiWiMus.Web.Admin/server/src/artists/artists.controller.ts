import {
    Body,
    Controller,
    Delete,
    Get,
    HttpException,
    HttpStatus,
    Param,
    Patch,
    Post,
    Query,
    UsePipes,
    ValidationPipe
} from '@nestjs/common';
import {FilterOptionsService} from "../filters/services/filter.options.service";
import {FilterOptions} from "../filters/filter.options";
import {Artist} from "./artist.entity";
import {UserArtist} from "../userArtist/userArtist.entity";
import {ArtistsDto} from "./dto/artists.dto";
import {ApiBearerAuth, ApiCreatedResponse, ApiOkResponse, ApiTags} from "@nestjs/swagger";
import {plainToInstance} from "class-transformer";
import {PaginatedData} from "../pagination/paginatied.data";
import {CreateArtistDto} from "./dto/create.artist.dto";
import {ArtistsService} from "./artists.service";
import {UserDto} from "../users/dto/user.dto";
import {UserArtistDto} from "./dto/user.artist.dto";
import {UpdateArtistDto} from "./dto/update.artist.dto";
import {GenreDto} from "../genres/dto/genre.dto";

@Controller('artists')
@ApiTags('artists')
@ApiBearerAuth('swagger')
export class ArtistsController {
    constructor(private readonly filterOptionsService: FilterOptionsService, 
                private readonly artistsService: ArtistsService){}

    @Post()
    @ApiCreatedResponse({ type: [ArtistsDto] })
    @UsePipes(new ValidationPipe({skipMissingProperties: true, whitelist: true}))
    async createArtist(@Body() dto: CreateArtistDto) : Promise<ArtistsDto> {
        return await this.artistsService.createArtist(dto);
    }

    @Get(':id')
    @ApiOkResponse({ type: ArtistsDto })
    async getArtistById(@Param('id') id : string) : Promise<ArtistsDto> {
        let artist = await Artist.findOne(+id, {relations: ['albums', 'tracks']});
        if(!artist){
            throw new HttpException({
                message: "The artist does not exist."
            }, HttpStatus.BAD_REQUEST)
        }

        let result = plainToInstance(ArtistsDto, artist);
        result.tracksCount = artist.tracks.length;
        return result;
    }

    @Get()
    @ApiOkResponse({ type: [ArtistsDto] })
    async getArtists(@Query() options : FilterOptions) : Promise<PaginatedData<ArtistsDto>> {
        let normalizedOptions = this.filterOptionsService.NormalizeOptions(options);
        let obj = this.filterOptionsService.GetFindOptionsObject(options, ['albums']);
        let data = await Artist.find(obj)
            .then(items => items.map(data => plainToInstance(ArtistsDto, data)))
            .catch(err => {
                throw new HttpException(
                    {message: err.message}, HttpStatus.BAD_REQUEST)
            });
        let count = await Artist.count({where: obj.where});
        return new PaginatedData<ArtistsDto>(data, normalizedOptions, count);
    }

    @Get(":id/users")
    @ApiOkResponse({ type: [UserDto] })
    async getArtistsUsers(@Param('id') id : string) : Promise<UserDto[]> {
        let artist = await Artist.findOne(+id);
        if (!artist) {
            throw new HttpException({
                message: "This user does nor exist"
            }, HttpStatus.UNPROCESSABLE_ENTITY);
        }
        return UserArtist.find({where: {artist: artist}, relations: ['user', 'artist']})
            .then(i => i.map((u) => (u.user)))
            .then(u => u.map(data => plainToInstance(UserDto, data)));
    }

    @Post(":id/users")
    @ApiCreatedResponse({ type: [UserDto] })
    @UsePipes(new ValidationPipe({skipMissingProperties: true, whitelist: true}))
    async addUsers(@Param('id') id : string, @Body() dto: UserArtistDto) : Promise<UserDto[]> {
        return await this.artistsService.addUsers(+id, dto);
    }

    @Delete(":id/users")
    @ApiOkResponse({ type: [UserDto] })
    @UsePipes(new ValidationPipe({skipMissingProperties: true, whitelist: true}))
    async deleteUsers(@Param('id') id : string, @Body() dto: UserArtistDto) : Promise<UserDto[]> {
        return await this.artistsService.deleteUsers(+id, dto)
            .catch(err => {
                throw new HttpException({
                    message: err.message
                }, HttpStatus.BAD_REQUEST)});
    }

    @Patch()
    @ApiCreatedResponse({ type: [ArtistsDto] })
    @UsePipes(new ValidationPipe({skipMissingProperties: true, whitelist: true}))
    async updateArtist(@Body() dto: UpdateArtistDto) {
        return await this.artistsService.updateArtist(dto)
            .catch(err => {
            throw new HttpException({
                message: err.message
            }, HttpStatus.BAD_REQUEST)});
    }

    @Delete(":id")
    @ApiOkResponse({ type: Boolean })
    @UsePipes(new ValidationPipe({skipMissingProperties: true, whitelist: true}))
    async deleteArtist(@Param('id') id : string) : Promise<boolean> {
       return await this.artistsService.deleteArtist(+id)
           .catch(err => {
               throw new HttpException({
                   message: err.message
               }, HttpStatus.BAD_REQUEST)});
    }
}
