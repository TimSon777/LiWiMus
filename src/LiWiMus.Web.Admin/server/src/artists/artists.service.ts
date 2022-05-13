import {Body, Injectable} from '@nestjs/common';
import {ArtistsPersonalDto} from "./dto/artists.personal.dto";
import {ArtistsDto} from "./dto/artists.dto";
import {Artist} from "./artist.entity";
import {User} from "../users/user.entity";
import {UserArtist} from "../userArtist/userArtist.entity";
import {DateSetterService} from "../shared/update.modifiedAt/set.date";

@Injectable()
export class ArtistsService {
    constructor(private readonly dateSetter: DateSetterService){}
    async updateArtist(dto: ArtistsDto) {

        await Artist.update({id: dto.id}, dto);

        return Artist.findOne(dto.id);
    }
}
