import {Module} from "@nestjs/common";
import {UsersController} from "./users.controller";
import {FilterService} from "../filters/services/filter.service";
import {SortService} from "../filters/services/sort.service";
import {PaginationService} from "../pagination/pagination.service";
import {FilterOptionsService} from "../filters/services/filter.options.service";
import { UsersService } from './users.service';
import {DateSetterService} from "../shared/setDate/set.date";

@Module({
    imports: [],
    controllers: [UsersController],
    providers: [FilterService, SortService, PaginationService, FilterOptionsService, UsersService, DateSetterService],
})
export class UsersModule {}