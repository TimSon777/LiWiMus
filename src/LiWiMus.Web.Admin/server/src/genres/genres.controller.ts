import {
    Body,
    Get,
    HttpException,
    HttpStatus, Param, Patch, Delete,
    Post,
    Query,
    UseInterceptors,
    UsePipes,
    ValidationPipe, Controller
} from '@nestjs/common';
import {GenreDto} from "./dto/genre.dto";
import {Track} from "../tracks/track.entity";
import {Genre} from "./genre.entity";
import {FilterOptionsService} from "../filters/services/filter.options.service";
import {FilterOptions} from "../filters/filter.options";
import {getRepository} from "typeorm";
import {GenresService} from "./genres.service";
import {TransformInterceptor} from "../transformInterceptor/transform.interceptor";
import {TrackDto} from "../tracks/dto/track.dto";
import {ApiBearerAuth, ApiCreatedResponse, ApiOkResponse, ApiTags} from "@nestjs/swagger";
import {Artist} from "../artists/artist.entity";
import {plainToInstance} from "class-transformer";
import {PaginatedData} from "../pagination/paginatied.data";
import {UpdateGenreDto} from "./dto/update.genre.dto";

@Controller('genres')
@ApiTags('genres')
@ApiBearerAuth('swagger')
export class GenresController {
    constructor(private readonly filterOptionsService: FilterOptionsService,
                private readonly genreService: GenresService) {
    }

    @Get(':id')
    @UseInterceptors(new TransformInterceptor(GenreDto))
    @ApiOkResponse({ type: GenreDto })
    async getGenreById(@Param('id') id : string) : Promise<GenreDto> {
        let genre = await Genre.findOne(+id, {relations: ['tracks']})
            .catch(err => {
                throw new HttpException({
                    message: err.message
                }, HttpStatus.BAD_REQUEST)
            });
        let result = plainToInstance(GenreDto, genre);
        result.tracksCount = genre.tracks.length;
        return result;
    }
    
    @Get()
    @ApiOkResponse({ type: [GenreDto] })
    async getGenres(@Query() options : FilterOptions)
        : Promise<PaginatedData<GenreDto>>
    {
        let normalizedOptions = this.filterOptionsService.NormalizeOptions(options);
        let obj = this.filterOptionsService.GetFindOptionsObject(options);
        let data = await Genre.find(obj)
            .then(items => items.map(data => plainToInstance(GenreDto, data)))
            .catch(err => {
                throw new HttpException({
                    message: err.message
                }, HttpStatus.BAD_REQUEST)});
        let count = await Genre.count({where: obj.where});
        return new PaginatedData<GenreDto>(data, normalizedOptions, count);
    }

    @Delete(':id')
    async delete(@Param('id') id : number) {
        let genre = await Genre.findOne({id: id});
        if (!genre) {
            throw new HttpException({
                message: "Genre was not found"
            }, HttpStatus.NOT_FOUND)
        }
        
        await Genre
            .remove(genre);
    }

    @Patch()
    @UsePipes(new ValidationPipe({skipMissingProperties: true, whitelist:true}))
    @ApiCreatedResponse({ type: GenreDto })
    async updateGenres(@Body() dto: UpdateGenreDto) 
        : Promise<GenreDto> {
        return this.genreService.updateGenre(dto);
    }
}
