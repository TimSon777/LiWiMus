import {Controller, Get, HttpException, HttpStatus, Param, Query} from '@nestjs/common';
import {User} from "./user.entity";
import {Filter} from "../filters/filter";
import {FilterService} from "../filters/services/filter.service";
import {Page} from "../pagination/page";
import {SortService} from "../filters/services/sort.service";
import {Sort} from "../filters/sort";
import {PaginationService} from "../pagination/pagination.service";


@Controller("users")
export class UsersController {
    constructor(private readonly filterService: FilterService,
                private readonly sortService: SortService,
                private readonly  paginationService: PaginationService){}
    @Get('getusers')
    //http://localhost:3001/admin/api/users/getusers?filters[0][columnName]=userName&filters[0][operator]=cnt&filters[0][value]=L&page[numberOfElementsOnPage]=3&page[pageNumber]=1&sorts[0][columnName]=emaill&sorts[0][order]=ASC
    async getUsers(@Query('filters') filters : Filter[],
                   @Query('page') page : Page = {numberOfElementsOnPage: 10, pageNumber: 1},
                   @Query('sorts') sorts : Sort[] = [{columnName: "id", order: "ASC"}])
        : Promise<User[]> 
    {
            return User.find({
                where: this.filterService.GetWhereObject(filters),
                skip: this.paginationService.GetSkipObject(page),
                take: this.paginationService.GetTakeObject(page),
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

