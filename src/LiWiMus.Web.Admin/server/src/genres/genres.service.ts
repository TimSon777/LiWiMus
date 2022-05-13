import { Injectable } from '@nestjs/common';
import {GenreDto} from "./dto/genre.dto";
import {Genre} from "./genre.entity";
import {Track} from "../tracks/track.entity";

@Injectable()
export class GenresService {
    
    async updateGenre(dto: GenreDto) : Promise<Genre> {
        if (await Genre.findOne(dto.id)){
            let updatedGenre = Genre.create({id: dto.id, name: dto.name});
          //  let today = new Date()
         //   updatedGenre.modifiedAt  = today.toISOString()
            await Genre.save(updatedGenre)
            return Genre.findOne(dto.id, {relations: ['tracks']})
        }
    }
}

function parseISOString(s) {
    let b = s.split(/\D+/);
    return new Date(Date.UTC(b[0], --b[1], b[2], b[3], b[4], b[5], b[6]));
}