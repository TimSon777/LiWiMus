import {Entity, Column, OneToOne, JoinColumn, OneToMany} from 'typeorm';
import {Artist} from "../artists/artist.entity";
import {UserRole} from "../userRoles/userRoles.entity";
import {ExternalLogin} from "../externalLogins/externalLogin.entity";
import {Transaction} from "../transactions/transaction.entity";
import {Playlist} from "../playlists/playlist.entity";
import {CommonEntity} from "../shared/commonEntity";
import {UserArtist} from "../userArtist/userArtist.entity";
import {Exclude} from "class-transformer";

@Entity('aspnetusers')
export class User extends CommonEntity {
  @Column({ name: 'FirstName', length: 50 })
  firstName: string;
  
  @Column({ name: 'SecondName', length: 50 })
  secondName: string;
  
  @Column({ name: 'Patronymic', length: 50 })
  patronymic: string;
  
  @Column({ type: 'enum', enum: ["Male","Female"], name: 'Gender' })
  gender: "Male" | "Female";
  
  @Column({ type: 'date', name: 'BirthDate' })
  birthDate: Date;
  
  @Column({ type: 'decimal',name: 'Balance' })
  balance: number;

  @Exclude()
  @Column({ name: 'AvatarPath' })
  avatarPath: string;
  
  @Column({ length: 256, name: 'Email' })
  email: string;
  
  @Column({ name: 'EmailConfirmed' })
  emailConfirmed: boolean;

  @Exclude()
  @Column({ name: 'NormalizedEmail', length: 256 })
  normalizedEmail: string;

  @Exclude()
  @Column({ name: 'NormalizedUserName', length: 20 })
  normalizedUserName: string;

  @Column({ name: 'UserName', length: 20 })
  userName: string;
  
  @Exclude()
  @Column({ name: 'PasswordHash' })
  passwordHash: string;

  @OneToMany(() => UserArtist, ua => ua.user)
  userArtists: UserArtist[];

  @OneToMany(() => UserRole, userRole => userRole.user, {onDelete: "CASCADE"})
  userRoles: UserRole[];

  @Exclude()
  @OneToMany(() => ExternalLogin, externalLogin => externalLogin.user, {onDelete: "CASCADE"})
  externalLogins: ExternalLogin[];

  @OneToMany(() => Transaction, transaction => transaction.user, {onDelete: "CASCADE"})
  transactions: Transaction[];

  @OneToMany(() => Playlist, playlist => playlist.owner, {onDelete: "CASCADE"})
  playlists: Playlist[];
}
