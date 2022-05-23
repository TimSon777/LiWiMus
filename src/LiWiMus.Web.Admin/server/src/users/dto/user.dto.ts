import {IdDto} from "../../shared/dto/id.dto";
import {
    IsBoolean,
    IsDateString,
    IsEmail,
    IsEnum,
    IsString,
    MaxLength
} from "class-validator";
import {Gender} from "../gender/gender";
import {Exclude, Expose} from "class-transformer";
import {ApiProperty} from "@nestjs/swagger";


@Exclude()
export class UserDto extends IdDto {
    @ApiProperty()
    @Expose()
    firstName: string;

    @ApiProperty()
    @Expose()
    secondName: string;

    @ApiProperty()
    @Expose()
    patronymic: string;

    @ApiProperty()
    @Expose()
    gender: Gender;

    @ApiProperty()
    @Expose()
    birthDate: Date;

    @ApiProperty()
    @Expose()
    @MaxLength(256)
    email: string;

    @ApiProperty()
    @Expose()
    emailConfirmed: boolean;

    @ApiProperty()
    @Expose()
    userName: string;

    @ApiProperty()
    @Expose()
    avatarLocation: string;

    @ApiProperty()
    @Expose()
    createdAt: Date;

    @ApiProperty()
    @Expose()
    modifiedAt: Date;

    @ApiProperty()
    @Expose()
    balance: number;
}


