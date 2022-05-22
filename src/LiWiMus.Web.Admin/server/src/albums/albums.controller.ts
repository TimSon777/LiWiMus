import {
    Body,
    Controller,
    Get,
    HttpException, HttpStatus, Param,
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
import {plainToInstance} from "class-transformer";
import {PaginatedData} from "../pagination/paginatied.data";

@Controller('albums')
@ApiTags('albums')
export class AlbumsController {
    constructor(private readonly filterOptionsService: FilterOptionsService,
                private readonly albumsService: AlbumsService) {
    }

    @Get(':id')
    @ApiOkResponse({ type: AlbumDto })
    async getAlbumById(@Param('id') id : string) : Promise<AlbumDto> {
        let album = Album.findOne(+id)
            .catch(err => {
                throw new HttpException({
                    message: err.message
                }, HttpStatus.BAD_REQUEST)
            });
        return plainToInstance(AlbumDto, album);
    }

    @Get()
    @ApiOkResponse({ type: [AlbumDto] })
    async getAlbums(@Query() options : FilterOptions)
        : Promise<PaginatedData<AlbumDto>>
    {
        let normalizedOptions = this.filterOptionsService.NormalizeOptions(options);
        let obj = this.filterOptionsService.GetFindOptionsObject(options);

        let data = await Album.find(obj)
            .then(items => items.map(data => plainToInstance(AlbumDto, data)))
            .catch(err => {
                throw new HttpException({
                    message: err.message
                }, HttpStatus.BAD_REQUEST)
            });

        let count = await Album.count({where: obj.where});
        return new PaginatedData<AlbumDto>(data, normalizedOptions, count);
    }
}


