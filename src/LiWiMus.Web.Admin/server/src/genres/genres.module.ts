import { Module } from '@nestjs/common';
import { GenresController } from './genres.controller';
import { GenresService } from './genres.service';
import {FilterService} from "../filters/services/filter.service";
import {SortService} from "../filters/services/sort.service";
import {PaginationService} from "../pagination/pagination.service";
import {FilterOptionsService} from "../filters/services/filter.options.service";
import {UsersController} from "../users/users.controller";
import {UsersService} from "../users/users.service";

@Module({
  imports: [],
  controllers: [GenresController],
  providers: [FilterService, SortService, PaginationService, FilterOptionsService, GenresController],
})
export class GenresModule {
  
}
