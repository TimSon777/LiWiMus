import {Controller, Get, HttpException, HttpStatus, Query} from '@nestjs/common';
import {FilterOptionsService} from "../filters/services/filter.options.service";
import {FilterOptions} from "../filters/filter.options";
import {Track} from "./track.entity";

@Controller('tracks')
export class TracksController {
    constructor(private readonly filterOptionsService: FilterOptionsService){}
    @Get('getall')
    async getTracks(@Query() options : FilterOptions)
        : Promise<Track[]> {
        return Track.find(
                this.filterOptionsService.GetFindOptionsObject(options, ["genres", "artists", "playlists"]))

            .catch(err => {
                throw new HttpException({
                    message: err.message
                }, HttpStatus.BAD_REQUEST)
            });
    }
}
