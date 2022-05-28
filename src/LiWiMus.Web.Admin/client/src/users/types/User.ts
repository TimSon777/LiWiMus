import { parseISO } from "date-fns";
import { Gender } from "./Gender";

export class User {
  constructor(
    id: string,
    userName: string | undefined,
    email: string,
    emailConfirmed: string,
    firstName: string | undefined,
    secondName: string | undefined,
    patronymic: string | undefined,
    birthDate: string | undefined,
    gender: Gender | undefined,
    balance: string,
    avatarLocation: string | undefined,
    createdAt: string,
    modifiedAt: string,
    lockoutEnd: string | undefined
  ) {
    this.id = +id;
    this.userName = userName;
    this.email = email;
    this.emailConfirmed = Boolean(emailConfirmed);
    this.firstName = firstName;
    this.secondName = secondName;
    this.patronymic = patronymic;
    this.birthDate = birthDate ? parseISO(birthDate) : undefined;
    this.gender = gender;
    this.balance = parseFloat(balance);
    this.avatarLocation = avatarLocation;
    this.createdAt = parseISO(createdAt);
    this.modifiedAt = parseISO(modifiedAt);
    this.lockoutEnd = lockoutEnd ? parseISO(lockoutEnd) : undefined;
  }

  id: number;
  userName?: string;
  email: string;
  emailConfirmed: boolean;

  firstName?: string;
  secondName?: string;
  patronymic?: string;
  birthDate?: Date;
  gender?: Gender;
  balance: number;
  avatarLocation?: string;

  lockoutEnd?: Date;

  createdAt: Date;
  modifiedAt: Date;
}
