import { Module } from '@nestjs/common';
import { AlbumsController } from './albums.controller';
import { AlbumsService } from './albums.service';
import {FilterOptionsService} from "../filters/services/filter.options.service";
import {FilterService} from "../filters/services/filter.service";
import {SortService} from "../filters/services/sort.service";
import {PaginationService} from "../pagination/pagination.service";
import {ArtistsService} from "../artists/artists.service";
import {DateSetterService} from "../shared/setDate/set.date";

@Module({
  controllers: [AlbumsController],
  providers: [AlbumsService, FilterOptionsService, FilterService, SortService, PaginationService, DateSetterService]
})
export class AlbumsModule {}
