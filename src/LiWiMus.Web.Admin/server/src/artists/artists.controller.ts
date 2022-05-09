import {Controller, Get, HttpException, HttpStatus, Query} from '@nestjs/common';
import {FilterOptionsService} from "../filters/services/filter.options.service";
import {FilterOptions} from "../filters/filter.options";
import {Artist} from "./artist.entity";

@Controller('artists')
export class ArtistsController {
    constructor(private readonly filterOptionsService: FilterOptionsService){}
    @Get('getall')
    async getArtists(@Query('options') options : FilterOptions)
        : Promise<Artist[]> {
        return Artist.find(
            this.filterOptionsService.GetFindOptionsObject(options, ["userArtists", "albums", "tracks" ]))
            
            .catch(err => {
                throw new HttpException({
                    message: err.message
                }, HttpStatus.BAD_REQUEST)
            });
    }
    
}
//http://localhost:3001/api/artists/getall?options[page][numberOfElementsOnPage]=3&options[page][pageNumber]=1&options[sorting][0][columnName]=id&options[sorting][0][order]=DESC&options[filters][0][columnName]=name&options[filters][0][operator]=cnt&options[filters][0][value]=n