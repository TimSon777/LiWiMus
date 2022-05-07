import {Column, ManyToOne} from "typeorm";
import {User} from "../../users/user.entity";

export class TransactionDto {
    userId: number;
    amount: number;
    description: string;
}