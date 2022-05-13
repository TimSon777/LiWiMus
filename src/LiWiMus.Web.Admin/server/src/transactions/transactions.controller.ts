import {Controller, Get, HttpException, HttpStatus, Query, UseInterceptors} from '@nestjs/common';
import {FilterOptionsService} from "../filters/services/filter.options.service";
import {FilterOptions} from "../filters/filter.options";
import {Transaction} from "./transaction.entity";
import {TransformInterceptor} from "../transformInterceptor/transform.interceptor";
import {TrackDto} from "../tracks/dto/track.dto";
import {TransactionDto} from "./dto/transaction.dto";

@Controller('transactions')
export class TransactionsController {
    constructor(private readonly filterOptionsService: FilterOptionsService){}
    @Get('getall')
    @UseInterceptors(new TransformInterceptor(TransactionDto))
    async getTransactions(@Query() options : FilterOptions) : Promise<TransactionDto[]>
        {
        return  Transaction.find(
            this.filterOptionsService.GetFindOptionsObject(options, ['user']))
            .catch(err => {
                throw new HttpException({
                    message: err.message
                }, HttpStatus.BAD_REQUEST)
            });
        
        
    }
}
