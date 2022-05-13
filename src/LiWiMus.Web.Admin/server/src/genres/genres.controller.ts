import {
    Body,
    Controller,
    Get,
    HttpException,
    HttpStatus,
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

@Controller('genres')
export class GenresController {
    constructor(private readonly filterOptionsService: FilterOptionsService,
                private readonly genreService: GenresService) {
    }
    
    @Get("getall")
    @UseInterceptors(new TransformInterceptor(GenreDto))
    async getAll(@Query() options : FilterOptions) : Promise<GenreDto[]> {
        return await Genre
            .find(this.filterOptionsService.GetFindOptionsObject(options, ['tracks']));
    }
    
    @Post("updateGenre")
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
