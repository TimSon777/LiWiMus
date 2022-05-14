import {Controller, Get, HttpException, HttpStatus, Query, UseInterceptors} from '@nestjs/common';
import {FilterOptionsService} from "../filters/services/filter.options.service";
import {FilterOptions} from "../filters/filter.options";
import {Track} from "./track.entity";
import {TransformInterceptor} from "../transformInterceptor/transform.interceptor";
import {TrackDto} from "./dto/track.dto";
import {ApiOkResponse, ApiTags} from "@nestjs/swagger";
import {Artist} from "../artists/artist.entity";

@Controller('tracks')
@ApiTags('tracks')
export class TracksController {
    constructor(private readonly filterOptionsService: FilterOptionsService){}
    @Get('getall')
    @UseInterceptors(new TransformInterceptor(TrackDto))
    @ApiOkResponse({ type: [Track] })
    async getTracks(@Query() options : FilterOptions)
        : Promise<Track[]> {
        return Track.find(
                this.filterOptionsService.GetFindOptionsObject(options, ['genres', 'artists', 'album']))

            .catch(err => {
                throw new HttpException({
                    message: err.message
                }, HttpStatus.BAD_REQUEST)
            });
    }
}
