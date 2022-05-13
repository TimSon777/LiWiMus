import {Column, ManyToOne} from "typeorm";
import {User} from "../../users/user.entity";
import {Exclude, Expose} from "class-transformer";
import {ValidateNested} from "class-validator";
import {UseInterceptors} from "@nestjs/common";
import {TransformInterceptor} from "../../transformInterceptor/transform.interceptor";
import {UserDto} from "../../users/dto/user.dto";

@Exclude()
export class TransactionDto {

    @Expose()
    user: User;
    
    @Expose()
    amount: number;
    
    @Expose()
    description: string;
}