import {BaseEntity, Column, Entity, ManyToOne, PrimaryColumn} from "typeorm";
import {User} from "../users/user.entity";

@Entity('aspnetuserlogins')
export class ExternalLogin extends BaseEntity {
    @PrimaryColumn({ name: 'LoginProvider', length: 128 })
    loginProvider: string

    @PrimaryColumn({ name: 'ProviderKey', length: 128 })
    providerKey: string
    
    @Column({ name: 'ProviderDisplayName'})
    providerDisplayName: string;

    @Column({ name: 'UserId', type: 'int' })
    @ManyToOne(() => User, user => user.externalLogins)
    user: User;
}