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
import {MoreThan} from "typeorm";
import {ApiBearerAuth} from "@nestjs/swagger";

@Controller()
@ApiBearerAuth('swagger')
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

  @Get('uaa')
  async aa(): Promise<Track[]> {
    return Track.find({relations: ['artists']});
  }

  @Get('uaaa')
  async aaa(): Promise<Artist[]> {
    return Artist.find({ relations: ["userArtists", "albums", "tracks"]});
  }

  @Get('uaaaa')
  async aaaa(): Promise<Artist[]> {
    // return Artist.find({
    //   join: {
    //     alias: 'A',
    //     leftJoin: {
    //       userArtists: 'userArtists'
    //     }
    //   } 
    // })
    return Artist.find({ skip: 0, take:10, relations: ["userArtists", "albums", "tracks"]});
  }

  @Get('qq')
  async qq(): Promise<Track[]> {
    // return Artist.find({
    //   join: {
    //     alias: 'A',
    //     leftJoin: {
    //       userArtists: 'userArtists'
    //     }
    //   } 
    // })
    return Track.find({ skip: 0, take:10, relations: ["artists", "album", "genres", "playlists"]});
  }
  
  @Get('q')
  async q(): Promise<User[]> {
    // return Artist.find({
    //   join: {
    //     alias: 'A',
    //     leftJoin: {
    //       userArtists: 'userArtists'
    //     }
    //   } 
    // })
    return User.find({ skip: 0, take:10, relations: ["userArtists", "externalLogins", "transactions", "playlists"]});
  }
  
  @Get('authtest')
  async fff(): Promise<string> {
    return "OKKKK";
  }
}
