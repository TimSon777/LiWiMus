import {HttpException, HttpStatus, Injectable} from '@nestjs/common';
import {DateSetterService} from "../shared/setDate/set.date";
import {PlaylistDto} from "./dto/playlist.dto";
import {CreatePlaylistDto} from "./dto/create.playlist.dto";
import {Playlist} from "./playlist.entity";
import {plainToInstance} from "class-transformer";
import {User} from "../users/user.entity";
import {UserDto} from "../users/dto/user.dto";
import {PlaylistTrackDto} from "./dto/playlist.track.dto";
import {UserArtist} from "../userArtist/userArtist.entity";
import {Track} from "../tracks/track.entity";
import {PlaylistTrack} from "../playlistTracks/playlistTrack.entity";
import {TrackDto} from "../tracks/dto/track.dto";

@Injectable()
export class PlaylistsService {
    constructor(private readonly dateSetter: DateSetterService) {}
    async createPlaylist(dto: CreatePlaylistDto) : Promise<PlaylistDto> {
        
        if (!dto.owner) {
            throw new HttpException({
                message: "Enter owner."
            }, HttpStatus.NOT_FOUND)
        }
        
        let owner = await User.findOne(dto.owner);
        if (!owner) {
            throw new HttpException({
                message: "Enter existing owner."
            }, HttpStatus.NOT_FOUND)
        }
        
        let playlist = Playlist.create({name: dto.name, isPublic: dto.isPublic, photoLocation: dto.photoLocation, owner: owner});

        let date = await this.dateSetter.setDate();
        playlist.createdAt = date;
        playlist.modifiedAt = date;
        
        await Playlist.save(playlist);

        return plainToInstance(PlaylistDto, Playlist.findOne(playlist.id, {relations: ['owner']}));
    }
    
    async addPlaylistTracks(id: number, dto: PlaylistTrackDto ) : Promise<TrackDto[]> {

        if (!dto.tracks) {
            throw new HttpException({
                message: "Enter tracks."
            }, HttpStatus.NOT_FOUND)
        }

        let playlist = await Playlist.findOne(id);
        if(!playlist) {
            throw new HttpException({
                message: "Playlist was not found."
            }, HttpStatus.NOT_FOUND)
        }
        
        let tracks = await Track.find({
            where: dto.tracks.map((id) => ({id} as Track))
        })
            .catch(err => {
                throw new HttpException({
                    message: "One of entered tracks was not found."
                }, HttpStatus.NOT_FOUND)
            });

        let playlistTracks: PlaylistTrack[] = [];

        for (let track  of tracks)  {
            let relation = await PlaylistTrack.findOne({where: {playlist: playlist, track: track}});
            if (relation){
                throw new HttpException({
                    message: `Playlist already have this track: id: ${track.id}`
                }, HttpStatus.CONFLICT)
            }


            let date = await this.dateSetter.setDate();
            playlist.modifiedAt = date;
            
            let playlistTrack = PlaylistTrack.create({
                track: track,
                playlist: playlist,
                createdAt: date,
                modifiedAt: date});

            playlistTracks.push(playlistTrack)
        }

        await PlaylistTrack.save(playlistTracks)
            .catch(err => {
                throw new HttpException({
                    message: err.message
                }, HttpStatus.BAD_REQUEST)
            });

        return PlaylistTrack.find({where: {playlist: playlist}, relations: ['playlist', 'track']})
            .then(i => i.map((u) => (u.track)))
            .then(p => p.map(track => plainToInstance(TrackDto, track)));
        
    }


    async deletePlaylistTrack(id : number, dto : PlaylistTrackDto) : Promise<TrackDto[]>{
        if (!dto.tracks) {
            throw new HttpException({
                message: "Enter tracks."
            }, HttpStatus.NOT_FOUND)
        }

        let playlist = await Playlist.findOne(id);
        if(!playlist) {
            throw new HttpException({
                message: "Playlist was not found."
            }, HttpStatus.NOT_FOUND)
        }

        let tracks = await Track.find({
            where: dto.tracks.map((id) => ({id} as Track))
        })
            .catch(err => {
                throw new HttpException({
                    message: "One of entered tracks was not found."
                }, HttpStatus.NOT_FOUND)
            });

        for(let track of tracks) {
            let relation = await PlaylistTrack.findOne({where: {playlist: playlist, track: track}});
            if (!relation){
                throw new HttpException({
                    message: `Relation was not found.`
                }, HttpStatus.NOT_FOUND);
            }

            let playlistTrack = await PlaylistTrack.find({where: {playlist: playlist, track: track}});
            await PlaylistTrack.remove(playlistTrack);
        }

        playlist.modifiedAt = await this.dateSetter.setDate();

        return PlaylistTrack.find({where: {playlist: playlist}, relations: ['playlist', 'track']})
            .then(i => i.map((u) => (u.track)))
            .then(p => p.map(track => plainToInstance(TrackDto, track)));
    }
}