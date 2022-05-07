import { Module } from '@nestjs/common';
import { TransactionsController } from './transactions.controller';
import {UsersController} from "../users/users.controller";
import {FilterService} from "../filters/services/filter.service";
import {SortService} from "../filters/services/sort.service";
import {PaginationService} from "../pagination/pagination.service";
import {FilterOptionsService} from "../filters/services/filter.options.service";
import {UsersService} from "../users/users.service";

@Module({
  controllers: [TransactionsController],
  imports: [],
  providers: [FilterService, SortService, PaginationService, FilterOptionsService]
})
export class TransactionsModule {}
