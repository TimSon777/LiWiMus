import {Module} from "@nestjs/common";
import {UsersController} from "./users.controller";
import { UsersService } from './users.service';
import {FilterService} from "../filters/services/filter.service";

@Module({
    imports: [],
    controllers: [UsersController],
    providers: [UsersService, FilterService],
})
export class UsersModule {}