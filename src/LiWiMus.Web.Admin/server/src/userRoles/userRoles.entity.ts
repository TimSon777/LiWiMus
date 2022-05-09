import {BaseEntity, Column, Entity, JoinColumn, ManyToOne, OneToMany, PrimaryColumn} from "typeorm";
import {User} from "../users/user.entity";
import {Role} from "../roles/role.entity";

@Entity('aspnetuserroles')
export class UserRole extends BaseEntity {
    @ManyToOne(() => User, user => user.userRoles)
    @PrimaryColumn({ name: 'UserId', type: 'int' })
    @JoinColumn({ name: 'UserId', referencedColumnName: 'id' })
    user: User;

    @PrimaryColumn({ name: 'RoleId', type: 'int' })
    role: Role;

    @Column({ name: 'GrantedAt', type: 'datetime' })
    grantedAt: Date;

    @Column({ name: 'ActiveUntil', type: 'datetime' })
    activeUntil: Date;
}