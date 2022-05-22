import {
    Body,
    Controller, Delete,
    Get,
    HttpException,
    HttpStatus,
    Param,
    Post,
    Query,
    UsePipes, ValidationPipe
} from '@nestjs/common';
import {ApiCreatedResponse, ApiOkResponse, ApiTags} from "@nestjs/swagger";
import {FilterOptionsService} from "../filters/services/filter.options.service";
import {FilterOptions} from "../filters/filter.options";
import {Playlist} from "./playlist.entity";
import {PlaylistDto} from "./dto/playlist.dto";
import {PlaylistsService} from "./playlists.service";
import {plainToInstance} from "class-transformer";
import {PaginatedData} from "../pagination/paginatied.data";
import {CreatePlaylistDto} from "./dto/create.playlist.dto";
import {UserDto} from "../users/dto/user.dto";
import {TrackDto} from "../tracks/dto/track.dto";
import {PlaylistTrackDto} from "./dto/playlist.track.dto";

@Controller('playlists')
@ApiTags('playlists')
export class PlaylistsController {
    constructor(private readonly filterOptionsService: FilterOptionsService,
                private readonly playlistsService: PlaylistsService) {
    }

    @Get(':id')
    @ApiOkResponse({ type: PlaylistDto })
    async getPlaylistById(@Param('id') id : string): Promise<PlaylistDto> {
        let playlist = Playlist.findOne(+id, {relations: ['owner']})
            .catch(err => {
                throw new HttpException({
                    message: err.message
                }, HttpStatus.BAD_REQUEST)});
        return plainToInstance(PlaylistDto, playlist);
    }


    @Get()
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
                }, HttpStatus.BAD_REQUEST)});
        let count = await Playlist.count({where: obj.where});
        return new PaginatedData<PlaylistDto>(data, normalizedOptions, count);
    }
    
    @Post()
    @ApiCreatedResponse({ type: [PlaylistDto] })
    @UsePipes(new ValidationPipe({skipMissingProperties: true, whitelist: true}))
    async createPlaylist(@Body() dto: CreatePlaylistDto) : Promise<PlaylistDto> {
        return await this.playlistsService.createPlaylist(dto)
            .catch(err => {
                throw new HttpException({
                    message: err.message
                }, HttpStatus.BAD_REQUEST)});
    }

    @Post(":id/tracks")
    @ApiCreatedResponse({ type: [UserDto] })
    @UsePipes(new ValidationPipe({skipMissingProperties: true, whitelist: true}))
    async addTracks(@Param('id') id : string, @Body() dto: PlaylistTrackDto) : Promise<TrackDto[]> {
        return await this.playlistsService.addPlaylistTracks(+id, dto)
            .catch(err => {
                throw new HttpException({
                    message: err.message
                }, HttpStatus.BAD_REQUEST)});
    }

    @Delete(":id/tracks")
    @ApiOkResponse({ type: [UserDto] })
    @UsePipes(new ValidationPipe({skipMissingProperties: true, whitelist: true}))
    async deleteTracks(@Param('id') id : string, @Body() dto: PlaylistTrackDto) : Promise<TrackDto[]> {
        return await this.playlistsService.deletePlaylistTrack(+id, dto)
            .catch(err => {
            throw new HttpException({
                message: err.message
            }, HttpStatus.BAD_REQUEST)});
    }
}


