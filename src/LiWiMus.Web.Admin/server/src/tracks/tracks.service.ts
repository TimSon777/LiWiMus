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
import {TrackAlbumDto} from "./dto/track.album.dto";
import {Album} from "../albums/album.entity";
import {TrackArtistDto} from "./dto/track.artist.dto";

@Injectable()
export class TracksService {

    constructor (private readonly dateSetter: DateSetterService){}
    
    public async updateTrack(dto: UpdateTrackDto) : Promise<TrackDto> {
        let track = await Track.findOne(dto.id);
        if (!track) {
            throw new HttpException({
                message: "Track does not exist."
            }, HttpStatus.BAD_REQUEST);
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
                message: "Track does not exist."
            }, HttpStatus.BAD_REQUEST);
        }

        await Track.remove(track);
        return !await Track.findOne(id);
    }
    
    public async addTrackGenre(id: number, dto: TrackGenreDto) 
        : Promise<TrackDto> 
    {
        if (!dto.genreId) {
            throw new HttpException({
                message: "Enter genre."
            }, HttpStatus.BAD_REQUEST)
        }
        
        let genre = await Genre.findOne(dto.genreId);
        
        if(!genre) {
            throw new HttpException({
                message: "Genre does not exist."
            }, HttpStatus.BAD_REQUEST)
        }
        
        let date = await this.dateSetter.setDate();
        let track = await Track.findOne(id, {relations: ['genres']});
        if (!track) {
            throw new HttpException({
                message: "Track does not exist."
            }, HttpStatus.BAD_REQUEST)
        }

        track.modifiedAt = date;
        track.genres.push(genre);
        await Track.save(track);
        return plainToInstance(TrackDto, Track.findOne(id, {relations: ['genres', 'album', 'artists']}));
    }

    public async deleteTrackGenre(id: number, dto: TrackGenreDto)
        : Promise<TrackDto>
    {
        if (!dto.genreId) {
            throw new HttpException({
                message: "Enter genre."
            }, HttpStatus.BAD_REQUEST)
        }

        let genre = await Genre.findOne(dto.genreId);

        if (!genre) {
            throw new HttpException({
                message: "Genre does not exist."
            }, HttpStatus.BAD_REQUEST)
        }

        let date = await this.dateSetter.setDate();
        let track = await Track.findOne(id, {relations: ['genres']});
        if (!track) {
            throw new HttpException({
                message: "Track does not exist."
            }, HttpStatus.BAD_REQUEST)
        }

        const index = track.genres.map(function (g) {
            return g.id;
        }).indexOf(dto.genreId);
        
        if (index > -1) {
            track.genres.splice(index, 1);
        }
        
        if (track.genres.length === 0) {
            throw new HttpException({
                message: "The track must have a genre."
            }, HttpStatus.CONFLICT)
        }

        track.modifiedAt = date;
        await Track.save(track);
        const updatedTrack = await Track.findOne(id, {relations: ['genres', 'album', 'artists']});
        return plainToInstance(TrackDto, updatedTrack);
    }
    
    public async updateTrackAlbum(id: number, dto: TrackAlbumDto)
        : Promise<TrackDto>
    {
        if (!dto.albumId) {
            throw new HttpException({
                message: "Enter album."
            }, HttpStatus.BAD_REQUEST)
        }

        let album = await Album.findOne(dto.albumId);

        if (!album) {
            throw new HttpException({
                message: "Album does not exist."
            }, HttpStatus.BAD_REQUEST)
        }

        let date = await this.dateSetter.setDate();
        let track = await Track.findOne(id, {relations: ['album']});
        if (!track) {
            throw new HttpException({
                message: "Track does not exist."
            }, HttpStatus.BAD_REQUEST)
        }

        track.modifiedAt = date;
        track.album = album;
        await Track.save(track);
        const updatedTrack = await Track.findOne(id, {relations: ['genres', 'album', 'artists']});
        return plainToInstance(TrackDto, updatedTrack);
    }

    public async addTrackArtist(id: number, dto: TrackArtistDto)
        : Promise<TrackDto>
    {
        if (!dto.artistId) {
            throw new HttpException({
                message: "Enter artist."
            }, HttpStatus.BAD_REQUEST)
        }

        let artist = await Artist.findOne(dto.artistId);

        if (!artist) {
            throw new HttpException({
                message: "Artist does not exist."
            }, HttpStatus.BAD_REQUEST)
        }

        let date = await this.dateSetter.setDate();
        let track = await Track.findOne(id, {relations: ['artists']});
        if (!track) {
            throw new HttpException({
                message: "Track does not exist."
            }, HttpStatus.BAD_REQUEST)
        }

        track.modifiedAt = date;
        track.artists.push(artist);
        await Track.save(track);
        let updatedTrack = await Track.findOne(id, {relations: ['genres', 'album', 'artists']});
        return plainToInstance(TrackDto, updatedTrack);
    }

    public async deleteTrackArtist(id: number, dto: TrackArtistDto)
        : Promise<TrackDto>
    {
        if (!dto.artistId) {
            throw new HttpException({
                message: "Enter artist."
            }, HttpStatus.BAD_REQUEST)
        }

        let artist = await Artist.findOne(dto.artistId);

        if (!artist) {
            throw new HttpException({
                message: "Artist does not exist."
            }, HttpStatus.BAD_REQUEST)
        }

        let date = await this.dateSetter.setDate();
        let track = await Track.findOne(id, {relations: ['artists']});
        if(!track) {
            throw new HttpException({
                message: "Track does not exist."
            }, HttpStatus.BAD_REQUEST)
        }

        const index = track.artists.map(function (g) {
            return g.id;
        }).indexOf(dto.artistId);

        if (index > -1) {
            track.artists.splice(index, 1);
        }

        if (track.artists.length === 0) {
            throw new HttpException({
                message: "The rack must have a genre."
            }, HttpStatus.CONFLICT)
        }

        track.modifiedAt = date;

        await Track.save(track);
        const updatedTrack = await Track.findOne(id, {relations: ['genres', 'album', 'artists']});

        return plainToInstance(TrackDto, updatedTrack);
    }
} 



