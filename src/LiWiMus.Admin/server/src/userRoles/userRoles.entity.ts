import {BaseEntity, Column, Entity, JoinColumn, ManyToOne, OneToMany, PrimaryColumn} from "typeorm";
import {User} from "../users/user.entity";
import {Role} from "../roles/role.entity";

@Entity('aspnetuserroles')
export class UserRole extends BaseEntity {
    @PrimaryColumn({ name: 'UserId', type: 'int' })
    @ManyToOne(() => User, user => user.userRoles)
    user: User;

    @PrimaryColumn({ name: 'RoleId', type: 'int' })
    role: Role;

    @Column({ name: 'GrantedAt', type: 'datetime' })
    grantedAt: Date;

    @Column({ name: 'ActiveUntil', type: 'datetime' })
    activeUntil: Date;
}