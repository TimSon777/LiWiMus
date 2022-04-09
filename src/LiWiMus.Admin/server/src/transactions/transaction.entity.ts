import {CommonEntity} from "../shared/commonEntity";
import {Column, Entity, ManyToOne} from "typeorm";
import {User} from "../users/user.entity";

@Entity('transactions')
export class Transaction extends CommonEntity {
    @Column({ name: 'UserId', type: 'int' })
    @ManyToOne(() => User, user => user.transactions)
    user: User

    @Column({ type: 'decimal', name: 'Amount' })
    amount: number;

    @Column({ name: 'Description' })
    description: number;
}