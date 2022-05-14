import { Module } from '@nestjs/common';
import { PlaylistsController } from './playlists.controller';
import { PlaylistsService } from './playlists.service';
import {FilterService} from "../filters/services/filter.service";
import {SortService} from "../filters/services/sort.service";
import {PaginationService} from "../pagination/pagination.service";
import {FilterOptionsService} from "../filters/services/filter.options.service";
import {GenresService} from "../genres/genres.service";
import {DateSetterService} from "../shared/setDate/set.date";

@Module({
  controllers: [PlaylistsController],
  providers: [FilterService, SortService, PaginationService, FilterOptionsService, PlaylistsService, DateSetterService]
})
export class PlaylistsModule {}
