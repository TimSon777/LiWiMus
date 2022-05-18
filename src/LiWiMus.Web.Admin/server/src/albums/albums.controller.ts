import {
    Body,
    Controller,
    Get,
    HttpException, HttpStatus,
    Post,
    Query,
    UseInterceptors,
    UsePipes,
    ValidationPipe
} from '@nestjs/common';
import {FilterOptionsService} from "../filters/services/filter.options.service";
import {TransformInterceptor} from "../transformInterceptor/transform.interceptor";
import {FilterOptions} from "../filters/filter.options";
import {Album} from "./album.entity";
import {AlbumDto} from "./dto/album.dto";
import {AlbumsService} from "./albums.service";
import {ApiOkResponse, ApiTags} from "@nestjs/swagger";
import {User} from "../users/user.entity";

@Controller('albums')
@ApiTags('albums')
export class AlbumsController {
    constructor(private readonly filterOptionsService: FilterOptionsService,
                private readonly albumsService: AlbumsService) {
    }

    @Get()
    @UseInterceptors(new TransformInterceptor(AlbumDto))
    @ApiOkResponse({ type: [Album] })
    async getGenres(@Query() options : FilterOptions) : Promise<Album[]> {
        return await Album
            .find(this.filterOptionsService.GetFindOptionsObject(options));
    }
}
