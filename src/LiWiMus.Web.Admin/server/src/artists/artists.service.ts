import {Body, HttpException, HttpStatus, Injectable} from '@nestjs/common';
import {DateSetterService} from "../shared/setDate/set.date";
import {ArtistsDto} from "./dto/artists.dto";
import {Artist} from "./artist.entity";
import {CreateArtistDto} from "./dto/create.artist.dto";
import {UserArtist} from "../userArtist/userArtist.entity";
import {User} from "../users/user.entity";
import {Exclude, plainToInstance} from "class-transformer";

@Injectable()
export class ArtistsService {
    constructor (private readonly dateSetter: DateSetterService){}
    
    async createArtist(dto: CreateArtistDto) : Promise<ArtistsDto> {

        let date = await this.dateSetter.setDate();

        let artist = Artist.create({name: dto.name, about: dto.about, photoLocation: dto.photoLocation});
        artist.createdAt = date;
        artist.modifiedAt = date;
        await Artist.save(artist);
        
        if (dto.userIds) {
            let users = await User.find({
                where: dto.userIds.map((id) => ({id} as User)) 
            })
                .catch(err => {
                throw new HttpException({
                    message: "One of entered users does not exist."
                }, HttpStatus.NOT_FOUND)
            });

            let userArtists: UserArtist[] = [];

            users.forEach(function (user)  {
                let userArtist = UserArtist.create({
                    user: user, 
                    artist: artist, 
                    createdAt: date,
                    modifiedAt: date});
                userArtists.push(userArtist)
            })

            await UserArtist.save(userArtists)
                .catch(err => {
                    throw new HttpException({
                        message: err.message
                    }, HttpStatus.BAD_REQUEST)
                });
        }
        
        return plainToInstance(ArtistsDto, Artist.findOne(artist.id));
    }
    
    
    
    
    async updateArtist(dto: ArtistsDto) {

        await Artist.update({id: dto.id}, dto);

        return plainToInstance(ArtistsDto, Artist.findOne(dto.id));
    }
    
    
}
