import {CommonEntity} from "../shared/commonEntity";
import {Column, Entity, JoinColumn, ManyToOne} from "typeorm";
import {User} from "../users/user.entity";

@Entity('transactions')
export class Transaction extends CommonEntity {
    @ManyToOne(() => User, user => user.transactions)
    @JoinColumn({ name: 'UserId', referencedColumnName: 'id' })
    user: User

    @Column({ type: 'decimal', name: 'Amount' })
    amount: number;

    @Column({ name: 'Description' })
    description: string;
}