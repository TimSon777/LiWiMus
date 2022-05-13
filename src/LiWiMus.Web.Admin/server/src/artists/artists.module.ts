import { Module } from '@nestjs/common';
import {FilterService} from "../filters/services/filter.service";
import {SortService} from "../filters/services/sort.service";
import {PaginationService} from "../pagination/pagination.service";
import {FilterOptionsService} from "../filters/services/filter.options.service";
import { ArtistsController } from './artists.controller';
import { ArtistsService } from './artists.service';
import {DateSetterService} from "../shared/update.modifiedAt/set.date";


@Module({
    imports: [],
    controllers: [ArtistsController],
    providers: [FilterOptionsService, FilterService, SortService, PaginationService, ArtistsService, DateSetterService],
})
export class ArtistsModule {}
