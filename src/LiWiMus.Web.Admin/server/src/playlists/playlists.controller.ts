import {Controller, Get, HttpException, HttpStatus, Param, Query, UseInterceptors} from '@nestjs/common';
import {ApiOkResponse, ApiTags} from "@nestjs/swagger";
import {FilterOptionsService} from "../filters/services/filter.options.service";
import {TransformInterceptor} from "../transformInterceptor/transform.interceptor";
import {FilterOptions} from "../filters/filter.options";
import {Playlist} from "./playlist.entity";
import {PlaylistDto} from "./dto/playlist.dto";
import {PlaylistsService} from "./playlists.service";
import {Artist} from "../artists/artist.entity";
import {plainToInstance} from "class-transformer";
import {PaginatedData} from "../pagination/paginatied.data";

@Controller('playlists')
@ApiTags('playlists')
export class PlaylistsController {
    constructor(private readonly filterOptionsService: FilterOptionsService,
                private readonly playlistsService: PlaylistsService) {
    }

    @Get(':id')
    @ApiOkResponse({ type: PlaylistDto })
    async getPlaylistById(@Param('id') id : string): Promise<PlaylistDto> {
        let playlist = Playlist.findOne(+id)
            .catch(err => {
                throw new HttpException({
                    message: err.message
                }, HttpStatus.BAD_REQUEST)
            });
        return plainToInstance(PlaylistDto, playlist);
    }


    @Get()
    //@UseInterceptors(new TransformInterceptor(PlaylistDto))
    @ApiOkResponse({ type: [PlaylistDto] })
    async getPlaylists(@Query() options : FilterOptions)
        : Promise<PaginatedData<PlaylistDto>>
    {
        let normalizedOptions = this.filterOptionsService.NormalizeOptions(options);
        let obj = this.filterOptionsService.GetFindOptionsObject(options, ['owner']);

        let data = await Playlist.find(obj)
            .then(items => items.map(data => plainToInstance(PlaylistDto, data)))
            .catch(err => {
                throw new HttpException({
                    message: err.message
                }, HttpStatus.BAD_REQUEST)
            });

        let count = await Playlist.count({where: obj.where});
        return new PaginatedData<PlaylistDto>(data, normalizedOptions, count);
    }
}


