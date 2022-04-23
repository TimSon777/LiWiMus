import {Controller, Get, HttpException, HttpStatus, Param, Query} from '@nestjs/common';
import {User} from "./user.entity";
import {Filter} from "../filters/filter";
import {FilterService} from "../filters/services/filter.service";
import {Page} from "../pagination/page";
import {SortService} from "../filters/services/sort.service";
import {Sort} from "../filters/sort";


@Controller("users")
export class UsersController {
    constructor(private readonly filterService: FilterService,
                private readonly sortService: SortService) {}
    @Get('getusers')
    async getUsers(@Query('filters') filters : Filter[],
                   @Query('page') page : Page = {numberOfElementsOnPage: 10, pageNumber: 1},
                   @Query('sorts') sorts : Sort[] = [{columnName: "id", order: "ASC"}])
        : Promise<User[]> 
    {
            return User.find({
                where: this.filterService.GetWhereObject(filters),
                take: page.numberOfElementsOnPage,
                skip: (page.pageNumber - 1) * page.numberOfElementsOnPage,
                order: this.sortService.GetOrderObject(sorts)
            })
                .catch(err =>
            {
                throw new HttpException({
                    message: err.message
                }, HttpStatus.BAD_REQUEST);
            });
    }
}

