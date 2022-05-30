import {Controller, Get, HttpException, HttpStatus, Param, Query, UseInterceptors} from '@nestjs/common';
import {FilterOptionsService} from "../filters/services/filter.options.service";
import {FilterOptions} from "../filters/filter.options";
import {Transaction} from "./transaction.entity";
import {TransactionDto} from "./dto/transaction.dto";
import {ApiOkResponse, ApiTags} from "@nestjs/swagger";
import {plainToInstance} from "class-transformer";
import {PaginatedData} from "../pagination/paginatied.data";

@Controller('transactions')
@ApiTags('transactions')
export class TransactionsController {
    constructor(private readonly filterOptionsService: FilterOptionsService){}

    @Get(':id')
    @ApiOkResponse({ type: TransactionDto })
    async getTransactionsDtoById(@Param('id') id : string) : Promise<TransactionDto> {
        let transaction = Transaction.findOne(+id, {relations: ["user"]})
            .catch(err => {
                throw new HttpException({
                    message: err.message
                }, HttpStatus.BAD_REQUEST)});
        return plainToInstance(TransactionDto, transaction);
    }


    @Get()
    @ApiOkResponse({ type: [TransactionDto] })
    async getTransactions(@Query() options : FilterOptions) : Promise<PaginatedData<TransactionDto>> {
        let normalizedOptions = this.filterOptionsService.NormalizeOptions(options);
        let obj = this.filterOptionsService.GetFindOptionsObject(options, ['user']);
        let data = await Transaction.find(obj)
            .then(items => items.map(data => plainToInstance(TransactionDto, data)))
            .catch(err => {
                throw new HttpException({
                    message: err.message
                }, HttpStatus.BAD_REQUEST)});
        let count = await Transaction.count({where: obj.where, relations: obj.relations});
        return new PaginatedData<TransactionDto>(data, normalizedOptions, count);
    }
}

