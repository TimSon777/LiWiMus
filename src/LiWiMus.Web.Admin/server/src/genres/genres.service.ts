import {HttpException, HttpStatus, Injectable} from '@nestjs/common';
import {GenreDto} from "./dto/genre.dto";
import {Genre} from "./genre.entity";
import {DateSetterService} from "../shared/setDate/set.date";
import {UpdateGenreDto} from "./dto/update.genre.dto";
import {plainToInstance} from "class-transformer";

@Injectable()
export class GenresService {
    constructor(private readonly dateSetter: DateSetterService) {}
    async updateGenre(dto: UpdateGenreDto) 
        : Promise<GenreDto> {
        if (await Genre.findOne(dto.id)) {
            let updatedGenre = Genre.create(dto);
            updatedGenre.modifiedAt = await this.dateSetter.setDate();
            await Genre.save(updatedGenre);
            let genre = await Genre.findOne(dto.id);
            return plainToInstance(GenreDto, genre);
        }
        else {
            throw new HttpException({
                message: "Genre does not exist."
            }, HttpStatus.BAD_REQUEST)
        }
    }
}