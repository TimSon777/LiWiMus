import {Body, Injectable} from '@nestjs/common';
import {DateSetterService} from "../shared/setDate/set.date";
import {ArtistsDto} from "./dto/artists.dto";
import {Artist} from "./artist.entity";

@Injectable()
export class ArtistsService {
    constructor (private readonly dateSetter: DateSetterService){}
    async updateArtist(dto: ArtistsDto) {

        await Artist.update({id: dto.id}, dto);

        return Artist.findOne(dto.id);
    }
}
