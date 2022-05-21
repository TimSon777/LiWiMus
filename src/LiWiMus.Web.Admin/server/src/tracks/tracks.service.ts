import {HttpException, HttpStatus, Injectable} from '@nestjs/common';
import {Track} from "./track.entity";
import {TrackDto} from "./dto/track.dto";
import {FilterOptions} from "../filters/filter.options";
import {FilterOptionsService} from "../filters/services/filter.options.service";
import {PlaylistTrack} from "../playlistTracks/playlistTrack.entity";
import {Artist} from "../artists/artist.entity";
import {UpdateTrackDto} from "./dto/update.track.dto";
import {Genre} from "../genres/genre.entity";
import {plainToInstance} from "class-transformer";
import {DateSetterService} from "../shared/setDate/set.date";
import {TrackGenreDto} from "./dto/track.genre.dto";
import {User} from "../users/user.entity";
import {UserArtist} from "../userArtist/userArtist.entity";
import {UserDto} from "../users/dto/user.dto";

@Injectable()
export class TracksService {

    constructor (private readonly dateSetter: DateSetterService){}
    
    public async updateTrack(dto: UpdateTrackDto) : Promise<TrackDto> {
        let track = await Track.findOne(dto.id);
        if (!track) {
            throw new HttpException({
                message: "Track was not found."
            }, HttpStatus.NOT_FOUND)
        }
        let updatedTrack = Track.create(dto);
        updatedTrack.modifiedAt = await this.dateSetter.setDate();
        await Track.save(updatedTrack);
        return plainToInstance(TrackDto, Track.findOne(dto.id));
    }

    async deleteTrack(id: number) {
        let track = await Track.findOne(id);
        if (!track){
            throw new HttpException({
                message: `Track was not found.`
            }, HttpStatus.NOT_FOUND);
        }

        await Track.remove(track);
        return !await Track.findOne(id);
    }
    
    public async addTrackGenre(id: number, dto: TrackGenreDto) 
        : Promise<TrackDto> 
    {
        
        if (!dto.genresId) {
            throw new HttpException({
                message: "Enter genres."
            }, HttpStatus.NOT_FOUND)
        }

        let date = await this.dateSetter.setDate();
        let track = await Track.findOne(id);
        if(!track) {
            throw new HttpException({
                message: "Track was not found."
            }, HttpStatus.NOT_FOUND)
        }

        track.modifiedAt = date;

        let genres = await Genre.find({
            where: dto.genresId.map((id) => ({id} as Genre))
        })
            .catch(err => {
                throw new HttpException({
                    message: "One of entered genres was not found."
                }, HttpStatus.NOT_FOUND)
            });

        let genreList: Genre[] = [];
        for (let genre of genres){
            genreList.push(genre);
        }
        
        track.genres = genreList;
        return plainToInstance(TrackDto, Track.findOne(id, {relations: ['genres']}));
    }

    public async deleteTrackGenre(id: number, dto: TrackGenreDto)
        
    {

    }
} 



