import {
    Body,
    Controller,
    Get,
    HttpException,
    HttpStatus, Param, Patch,
    Post,
    Query,
    UseInterceptors,
    UsePipes,
    ValidationPipe
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
import {ApiOkResponse, ApiTags} from "@nestjs/swagger";
import {Artist} from "../artists/artist.entity";
import {plainToInstance} from "class-transformer";

@Controller('genres')
@ApiTags('genres')
export class GenresController {
    constructor(private readonly filterOptionsService: FilterOptionsService,
                private readonly genreService: GenresService) {
    }

    @Get(':id')
    @UseInterceptors(new TransformInterceptor(GenreDto))
    @ApiOkResponse({ type: GenreDto })
    async getGenreById(@Param('id') id : string) : Promise<GenreDto> {
        let genre = Genre.findOne(+id)
            .catch(err => {
                throw new HttpException({
                    message: err.message
                }, HttpStatus.BAD_REQUEST)
            });
        return plainToInstance(GenreDto, genre);
    }
    
    @Get("getall")
    @UseInterceptors(new TransformInterceptor(GenreDto))
    @ApiOkResponse({ type: [GenreDto] })
    async getGenres(@Query() options : FilterOptions) : Promise<Genre[]> {
        return await Genre
            .find(this.filterOptionsService.GetFindOptionsObject(options));
    }
    
    @Patch("updateGenre")
    @UsePipes(new ValidationPipe({skipMissingProperties: true, whitelist:true}))
    async updateGenres(@Body() dto: GenreDto) : Promise<Genre> {
        return this.genreService.updateGenre(dto)
            .catch(err => {
            throw new HttpException({
                message: err.message
            }, HttpStatus.BAD_REQUEST)
        });
    }
}
