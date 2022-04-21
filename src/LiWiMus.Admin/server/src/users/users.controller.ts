import {Controller, Get, Param, Query} from '@nestjs/common';
import {User} from "./user.entity";
import { UsersService } from './users.service';
import {json} from "express";
import {Filter} from "../filters/filter";
import {FilterService} from "../filters/services/filter.service";


@Controller("users")
export class UsersController {
    constructor(private readonly usersService: UsersService, private readonly filterService: FilterService) {}
//http://localhost:3001/admin/api/users/getusers?userName=landysh&email=pep&date=11-09-2022
    //getusers/:page/:usersOnPage
    @Get('getusers')
    async getUsers(@Query() filters : Filter[])
        : Promise<User[]> 
    {
        return User.find({
            where: this.filterService.GetWhereObject(filters),
        });
        
    }
}

///getusers?_page=3&_sort=id&_order=asc&_undefined=undefined

