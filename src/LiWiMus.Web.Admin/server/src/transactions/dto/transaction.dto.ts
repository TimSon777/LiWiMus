import {Column, ManyToOne} from "typeorm";
import {User} from "../../users/user.entity";
import {Exclude, Expose, Type} from "class-transformer";
import {IsNumber, IsString, ValidateNested} from "class-validator";
import {UseInterceptors} from "@nestjs/common";
import {TransformInterceptor} from "../../transformInterceptor/transform.interceptor";
import {UserDto} from "../../users/dto/user.dto";
import {IdDto} from "../../shared/dto/id.dto";
import {ApiProperty} from "@nestjs/swagger";

@Exclude()
export class TransactionDto extends IdDto {
    @ApiProperty()
    @Expose()
    @ValidateNested()
    @Type(() => UserDto)
    user: UserDto;

    @ApiProperty()
    @Expose()
    @IsNumber()
    amount: number;

    @ApiProperty()
    @Expose()
    @IsString()
    description: string;

    @ApiProperty()
    @Expose()
    createdAt: Date;

    @ApiProperty()
    @Expose()
    modifiedAt: Date;
}