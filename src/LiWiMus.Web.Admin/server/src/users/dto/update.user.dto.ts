import {IdDto} from "../../shared/dto/id.dto";
import {
    IsArray,
    IsBoolean,
    IsDateString,
    IsEmail,
    IsEnum,
    IsNumber,
    IsPositive,
    IsString,
    MaxLength
} from "class-validator";
import {Gender} from "../gender/gender";
import {ApiProperty} from "@nestjs/swagger";
import {Type} from "class-transformer";

export class UpdateUserDto extends IdDto {
    @ApiProperty()
    @MaxLength(50)
    @IsString()
    firstName: string;

    @ApiProperty()
    @MaxLength(50)
    @IsString()
    secondName: string;

    @ApiProperty()
    @MaxLength(50)
    @IsString()
    patronymic: string;

    @ApiProperty()
    @IsEnum(Gender)
    gender: Gender;

    @ApiProperty()
    @IsDateString()
    birthDate: Date;

    @ApiProperty()
    @IsEmail()
    @MaxLength(256)
    @IsString()
    email: string;

    @ApiProperty()
    @IsBoolean()
    emailConfirmed: boolean;

    @ApiProperty()
    @MaxLength(20)
    @IsString()
    userName: string;
}