import {
    Body,
    Controller,
    Get,
    HttpException,
    HttpStatus,
    Param,
    Patch, Post,
    Query,
    UseInterceptors,
    UsePipes, ValidationPipe
} from '@nestjs/common';
import {FilterOptionsService} from "../filters/services/filter.options.service";
import {FilterOptions} from "../filters/filter.options";
import {Track} from "./track.entity";
import {TransformInterceptor} from "../transformInterceptor/transform.interceptor";
import {TrackDto} from "./dto/track.dto";
import {ApiCreatedResponse, ApiOkResponse, ApiTags} from "@nestjs/swagger";
import {Artist} from "../artists/artist.entity";
import {User} from "../users/user.entity";
import {UserDto} from "../users/dto/user.dto";
import {PaginatedData} from "../pagination/paginatied.data";
import {plainToInstance} from "class-transformer";
import {UpdateTrackDto} from "./dto/update.track.dto";
import {TracksService} from "./tracks.service";
import {TrackGenreDto} from "./dto/track.genre.dto";

@Controller('tracks')
@ApiTags('tracks')
export class TracksController {
    constructor(private readonly filterOptionsService: FilterOptionsService,
                private  readonly trackService: TracksService){}

    @Get(':id')
    @ApiOkResponse({ type: TrackDto })
    async getTrackById(@Param('id') id : string) : Promise<TrackDto> {
        let track = Track.findOne(+id)
            .catch(err => {
                throw new HttpException({
                    message: err.message
                }, HttpStatus.BAD_REQUEST)
            });
        return plainToInstance(TrackDto, track);
    }


    @Get()
   // @UseInterceptors(new TransformInterceptor(TrackDto))
    @ApiOkResponse({ type: [TrackDto] })
    async getTracks(@Query() options : FilterOptions)
        : Promise<PaginatedData<TrackDto>> {

        let normalizedOptions = this.filterOptionsService.NormalizeOptions(options);
        let obj = this.filterOptionsService.GetFindOptionsObject(options, ['artists', 'album']);

        let data = await Track.find(obj)
            .then(items => items.map(data => plainToInstance(TrackDto, data)))
            .catch(err => {
                throw new HttpException({
                    message: err.message
                }, HttpStatus.BAD_REQUEST)
            });

        let count = await Track.count({where: obj.where});
        return new PaginatedData<TrackDto>(data, normalizedOptions, count);

    }
    
    @Patch()
    @UsePipes(new ValidationPipe({skipMissingProperties: true, whitelist: true}))
    @ApiCreatedResponse({ type: TrackDto })
    async updateTrack(@Body() dto: UpdateTrackDto) : Promise<TrackDto> {
        return await this.trackService.updateTrack(dto);
    }
    
    @Post(":id/genres")
    @UsePipes(new ValidationPipe({skipMissingProperties: true, whitelist: true}))
    @ApiCreatedResponse({ type: TrackDto })
    async addGenres(@Param('id') id: string, @Body() dto: TrackGenreDto) {
        return await this.trackService.addTrackGenre(+id, dto);
    }
}
