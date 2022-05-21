import { Module } from '@nestjs/common';
import { TracksController } from './tracks.controller';
import {ArtistsController} from "../artists/artists.controller";
import {FilterOptionsService} from "../filters/services/filter.options.service";
import {FilterService} from "../filters/services/filter.service";
import {SortService} from "../filters/services/sort.service";
import {PaginationService} from "../pagination/pagination.service";
import { TracksService } from './tracks.service';
import {DateSetterService} from "../shared/setDate/set.date";

@Module({
  controllers: [TracksController],
  providers: [FilterOptionsService, FilterService, SortService, PaginationService, TracksService, DateSetterService]
})
export class TracksModule {}
