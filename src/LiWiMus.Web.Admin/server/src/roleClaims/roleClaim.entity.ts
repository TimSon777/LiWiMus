import {BaseEntity, Column, Entity, JoinColumn, ManyToOne, PrimaryGeneratedColumn} from "typeorm";
import {Role} from "../roles/role.entity";

@Entity('aspnetroleclaims')
export class RoleClaim extends BaseEntity {
    @PrimaryGeneratedColumn({ name: 'Id' })
    id: number;

    @ManyToOne(() => Role, role => role.roleClaims)
    @JoinColumn({ name: 'RoleId', referencedColumnName: 'id' })
    role: Role;
    
    @Column({ name: 'ClaimType' })
    claimType: string;

    @Column({ name: 'ClaimValue' })
    claimValue: string;
}