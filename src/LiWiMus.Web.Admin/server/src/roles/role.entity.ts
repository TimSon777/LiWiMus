import {BaseEntity, Column, Entity, OneToMany, PrimaryGeneratedColumn} from "typeorm";
import {RoleClaim} from "../roleClaims/roleClaim.entity";

@Entity('aspnetroles')
export class Role extends BaseEntity {
    @PrimaryGeneratedColumn({ name: 'Id' })
    id: number;
    
    @Column({ name: 'Name', length: 256 })
    name: string;

    @Column({ name: 'NormalizedName', length: 256 })
    normalizedName: string;

    @Column({ name: 'Description' })
    description: string;

    @Column({ name: 'IsPublic' })
    isPublic: boolean;

    @Column({ name: 'PricePerMonth', type: 'decimal' })
    pricePerMonth: number;

    @Column({ name: 'DefaultTimeOut', type: 'datetime' })
    defaultTimeOut: Date;
    
    @OneToMany(() => RoleClaim, roleClaim => roleClaim.role)
    roleClaims: RoleClaim[];
}