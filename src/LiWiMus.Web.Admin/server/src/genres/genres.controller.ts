import {Body, Controller, Get, HttpException, HttpStatus, Post, Query, UsePipes, ValidationPipe} from '@nestjs/common';
import {GenreDto} from "./dto/genre.dto";
import {Track} from "../tracks/track.entity";
import {Genre} from "./genre.entity";
import {FilterOptionsService} from "../filters/services/filter.options.service";
import {FilterOptions} from "../filters/filter.options";
import {getRepository} from "typeorm";

@Controller('genres')
export class GenresController {
    constructor(private readonly filterOptionsService: FilterOptionsService) {
    }
    @Get("getall")
    async getAll(@Query('options') options : FilterOptions) {
        return await Genre
            .find(this.filterOptionsService.GetFindOptionsObject(options, ['tracks']));
    }
    
    @Post("updateGenre")
    @UsePipes(new ValidationPipe({skipMissingProperties: true}))
    async updateGenres(@Body() dto: GenreDto) : Promise<Genre> {
        let tracks = await Track.find({
            where: dto.tracksId.map((id) => ({ id } as Track))
        })
        let updatedGenre = Genre.create({id: dto.id, name: dto.name, tracks: tracks});
        await Genre.save(updatedGenre)
            .catch(err => {
            throw new HttpException({
                message: err.message
            }, HttpStatus.BAD_REQUEST)
        });
        return Genre.findOne(dto.id, {relations: ['tracks']})
    }
}
