import {BaseEntity, Column, Entity, ManyToOne, PrimaryGeneratedColumn} from "typeorm";
import {Role} from "../roles/role.entity";

@Entity('aspnetroleclaims')
export class RoleClaim extends BaseEntity {
    @PrimaryGeneratedColumn({ name: 'Id' })
    id: number;
    
    @Column({ name: 'RoleId', type: 'int' })
    @ManyToOne(() => Role, role => role.roleClaims)
    role: Role;
    
    @Column({ name: 'ClaimType' })
    claimType: string;

    @Column({ name: 'ClaimValue' })
    claimValue: string;
}