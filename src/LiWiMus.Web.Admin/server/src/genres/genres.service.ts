import { Injectable } from '@nestjs/common';
import {GenreDto} from "./dto/genre.dto";
import {Genre} from "./genre.entity";
import {DateSetterService} from "../shared/setDate/set.date";

@Injectable()
export class GenresService {
    constructor(private readonly dateSetter: DateSetterService) {
    }
    async updateGenre(dto: GenreDto) : Promise<Genre> {
        if (await Genre.findOne(dto.id)){
            let updatedGenre = Genre.create(dto);
            updatedGenre.modifiedAt = await this.dateSetter.setDate();
            await Genre.save(updatedGenre);
            return Genre.findOne(dto.id)
        }
    }
}