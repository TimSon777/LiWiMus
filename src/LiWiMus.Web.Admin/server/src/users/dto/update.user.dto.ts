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
import {Exclude, Expose, Type} from "class-transformer";

@Exclude()
export class UpdateUserDto extends IdDto {
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
    @IsString()
    avatarLocation: string;
}