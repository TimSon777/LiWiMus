import { AppService } from './app.service';
import {Controller, Get, Param} from '@nestjs/common';
import {User} from "./users/user.entity";
import {UserRole} from "./userRoles/userRoles.entity";
import {Genre} from "./genres/genre.entity";
import {RoleClaim} from "./roleClaims/roleClaim.entity";
import {Track} from "./tracks/track.entity";
import {Artist} from "./artists/artist.entity";
import {Playlist} from "./playlists/playlist.entity";
import {serialize} from "v8";
import {UserArtist} from "./userArtist/userArtist.entity";

@Controller()
export class AppController {
  constructor(private readonly appService: AppService) {}

  @Get()
  getHello(): string {
    return this.appService.getHello();
  }

  @Get('test')
  test(): string {
    return 'test';
  }

  @Get('getuser/:id')
  async getUser(@Param('id') id: number): Promise<User> {
    return User.findOne(id, {relations: ['artist', 'userRoles', 'externalLogins', 'transactions']});
  }

  @Get('getusers')
  async getUsers(): Promise<User[]> {
    return User.find();
  }
  
  @Get('getuserroles')
  async getRoles(): Promise<UserRole[]> {
    return UserRole.find();
  }

  @Get('getgenres')
  async getGenres(): Promise<Genre[]> {
    return Genre.find();
  }

  @Get('getclaimsroles')
  async getGenres1(): Promise<RoleClaim[]> {
    return RoleClaim.find();
  }

  @Get('gettracks')
  async gettracks(): Promise<Track[]> {
    return Track.find({relations: ['genres']});
  }

  @Get('getartists')
  async getartists(): Promise<Artist[]> {
    return Artist.find({relations: ['albums', 'tracks']});
  }

  @Get('play')
  async play(): Promise<Playlist[]> {
    return Playlist.find({relations: ['tracks']});
  }

  @Get('sex')
  async sex(): Promise<boolean> {
    return (await User.findOne(1)).gender === "Female";
  }

  @Get('ua')
  async ua(): Promise<UserArtist[]> {
    return UserArtist.find({relations: ['user', 'artist']});
  }
}
