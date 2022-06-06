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
}
