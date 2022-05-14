import {Controller, Get, Query, UseInterceptors} from '@nestjs/common';
import {ApiOkResponse, ApiTags} from "@nestjs/swagger";
import {FilterOptionsService} from "../filters/services/filter.options.service";
import {TransformInterceptor} from "../transformInterceptor/transform.interceptor";
import {FilterOptions} from "../filters/filter.options";
import {Playlist} from "./playlist.entity";
import {PlaylistDto} from "./dto/playlist.dto";
import {PlaylistsService} from "./playlists.service";
import {Artist} from "../artists/artist.entity";

@Controller('playlists')
@ApiTags('playlists')
export class PlaylistsController {
    constructor(private readonly filterOptionsService: FilterOptionsService,
                private readonly playlistsService: PlaylistsService) {
    }

    @Get("getall")
    @UseInterceptors(new TransformInterceptor(PlaylistDto))
    @ApiOkResponse({ type: [Playlist] })
    async getGenres(@Query() options : FilterOptions) : Promise<Playlist[]> {
        return await Playlist
            .find(this.filterOptionsService.GetFindOptionsObject(options, ['owner']));
    }
}
