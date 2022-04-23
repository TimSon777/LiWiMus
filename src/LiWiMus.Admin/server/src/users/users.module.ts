import {Module} from "@nestjs/common";
import {UsersController} from "./users.controller";
import {FilterService} from "../filters/services/filter.service";
import {SortService} from "../filters/services/sort.service";

@Module({
    imports: [],
    controllers: [UsersController],
    providers: [FilterService, SortService],
})
export class UsersModule {}