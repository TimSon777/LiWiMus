import {HttpException, HttpStatus, Injectable} from '@nestjs/common';
import {DateSetterService} from "../shared/setDate/set.date";
import {ArtistsDto} from "./dto/artists.dto";
import {Artist} from "./artist.entity";
import {CreateArtistDto} from "./dto/create.artist.dto";
import {UserArtist} from "../userArtist/userArtist.entity";
import {User} from "../users/user.entity";
import {plainToInstance} from "class-transformer";
import {UserArtistDto} from "./dto/user.artist.dto";
import {UserDto} from "../users/dto/user.dto";
import {UpdateArtistDto} from "./dto/update.artist.dto";

@Injectable()
export class ArtistsService {
    constructor (private readonly dateSetter: DateSetterService){}
    
    async createArtist(dto: CreateArtistDto) : Promise<ArtistsDto> {
        let artist = Artist.create({name: dto.name, about: dto.about, photoLocation: dto.photoLocation});
        let date = await this.dateSetter.setDate();
        artist.createdAt = date;
        artist.modifiedAt = date;
        await Artist.save(artist);
        return plainToInstance(ArtistsDto, Artist.findOne(artist.id));
    }
    
    async addUsers(id: number, dto: UserArtistDto) : Promise<UserDto[]>{
        if (!dto.userIds) {
            throw new HttpException({
                message: "Enter users."
            }, HttpStatus.NOT_FOUND)
        }
        
        let artist = await Artist.findOne(id);
        if(!artist) {
            throw new HttpException({
                message: "Artist was not found."
            }, HttpStatus.NOT_FOUND)
        }
    
        let users = await User.find({
            where: dto.userIds.map((id) => ({id} as User))
        })
            .catch(err => {
                throw new HttpException({
                    message: "One of entered users was not found."
                }, HttpStatus.NOT_FOUND)
            });

        let userArtists: UserArtist[] = [];

        for (let user  of users) {
            let relation = await UserArtist.findOne({where: {artist: artist, user: user}});
            if (relation) {
                throw new HttpException({
                    message: `Artist already have this user: id: ${user.id}`}, 
                    HttpStatus.CONFLICT)
            }
            let date = await this.dateSetter.setDate();
            artist.modifiedAt = date;
            let userArtist = UserArtist.create({user: user, artist: artist, createdAt: date, modifiedAt: date});
            userArtists.push(userArtist)
        }
        
        await UserArtist.save(userArtists)
            .catch(err => {
                throw new HttpException({
                    message: err.message
                }, HttpStatus.BAD_REQUEST)});
        return UserArtist.find({where: {artist: artist}, relations: ['user', 'artist']})
            .then(i => i.map((u) => (u.user)))
            .then(u => u.map(data => plainToInstance(UserDto, data)));
    }


    async deleteUsers(id : number, dto : UserArtistDto) : Promise<UserDto[]>{
        if (!dto.userIds) {
            throw new HttpException({
                message: "Enter users."
            }, HttpStatus.NOT_FOUND)
        }
        
        let date = await this.dateSetter.setDate();
        let artist = await Artist.findOne(id);
        if(!artist) {
            throw new HttpException({
                message: "Artist was not found."
            }, HttpStatus.UNPROCESSABLE_ENTITY)
        }
        
        let users = await User.find({
            where: dto.userIds.map((id) => ({id} as User))})
            .catch(err => {
                throw new HttpException({
                    message: "One of entered users was not found."
                }, HttpStatus.NOT_FOUND)});
        
        for (let user of users) {
            let relation = await UserArtist.findOne({where: {artist: artist, user: user}});
            if (!relation){
                throw new HttpException({
                    message: `Relation was not found.`
                }, HttpStatus.NOT_FOUND);
            }
            
            let userArtist = await UserArtist.find({where: {user: user, artist: artist}});
            await UserArtist.remove(userArtist);
        }

        artist.modifiedAt = date;
        return UserArtist.find({where: {artist: artist}, relations: ['user', 'artist']})
            .then(i => i.map((u) => (u.user)))
            .then(u => u.map(data => plainToInstance(UserDto, data)));
    }
    
    async updateArtist(dto: UpdateArtistDto) : Promise<ArtistsDto> {
        let artist = await Artist.findOne(dto.id);
        if (!artist){
            throw new HttpException({
                message: `Artist was not found.`
            }, HttpStatus.UNPROCESSABLE_ENTITY);
        }
        
        let updatedArtist = await Artist.create(dto);
        updatedArtist.modifiedAt = await this.dateSetter.setDate();
        await Artist.save(updatedArtist);
        return plainToInstance(ArtistsDto, Artist.findOne(dto.id));
    }
    
    async deleteArtist(id: number) {
        let artist = await Artist.findOne(id);
        if (!artist){
            throw new HttpException({
                message: `Artist was not found.`
            }, HttpStatus.UNPROCESSABLE_ENTITY);
        }
        
        await Artist.remove(artist);
        return !await Artist.findOne(id);
    }
}
