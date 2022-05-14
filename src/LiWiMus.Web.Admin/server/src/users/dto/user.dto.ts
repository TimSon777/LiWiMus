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
    @MaxLength(50)
    @IsString()
    firstName: string;

    @ApiProperty()
    @Expose()
    @MaxLength(50)
    @IsString()
    secondName: string;

    @ApiProperty()
    @Expose()
    @MaxLength(50)
    @IsString()
    patronymic: string;

    @ApiProperty()
    @Expose()
    @IsEnum(Gender)
    gender: Gender;

    @ApiProperty()
    @Expose()
    @IsDateString()
    birthDate: Date;

    @ApiProperty()
    @Expose()
    @MaxLength(256)
    @IsString()
    @IsEmail()
    email: string;

    @ApiProperty()
    @Expose()
    @IsBoolean()
    emailConfirmed: boolean;

    @ApiProperty()
    @Expose()
    @MaxLength(20)
    @IsString()
    userName: string;

    @ApiProperty()
    @Expose()
    createdAt: Date;

    @ApiProperty()
    @Expose()
    modifiedAt: Date;
}


