import {Controller, Get, HttpException, HttpStatus, Query} from '@nestjs/common';
import {FilterOptionsService} from "../filters/services/filter.options.service";
import {FilterOptions} from "../filters/filter.options";
import {Transaction} from "./transaction.entity";

@Controller('transactions')
export class TransactionsController {
    constructor(private readonly filterOptionsService: FilterOptionsService){}
    @Get('getall')
    async getUsers(@Query('options') options : FilterOptions)
        : Promise<Transaction[]> {
        return Transaction.find(
            this.filterOptionsService.GetFindOptionsObject(options, ['user']))
            .catch(err => {
                throw new HttpException({
                    message: err.message
                }, HttpStatus.BAD_REQUEST)
            });
    }
}
