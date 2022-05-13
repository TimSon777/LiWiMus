import { BaseEntity, Column, PrimaryGeneratedColumn } from 'typeorm';
import {Exclude, Expose} from "class-transformer";

export class CommonEntity extends BaseEntity {
  @PrimaryGeneratedColumn({ name: 'Id' })
  id: number;

  @Column({ name: 'CreatedAt', type: 'datetime' })
  createdAt: Date;
  
  @Column({ name: 'ModifiedAt', type: 'datetime' })
  modifiedAt: Date;
}
