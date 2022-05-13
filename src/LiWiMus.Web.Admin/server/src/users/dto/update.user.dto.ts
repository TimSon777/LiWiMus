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

export class UpdateUserDto extends IdDto {
    @MaxLength(50)
    @IsString()
    firstName: string;

    @MaxLength(50)
    @IsString()
    secondName: string;

    @MaxLength(50)
    @IsString()
    patronymic: string;

    @IsEnum(Gender)
    gender: Gender;

    @IsDateString()
    birthDate: Date;

    @IsEmail()
    @MaxLength(256)
    @IsString()
    email: string;

    @IsBoolean()
    emailConfirmed: boolean;

    @MaxLength(20)
    @IsString()
    userName: string;
    
    @IsNumber()
    @IsPositive({each: true})
    playlistsId: number[];

    @IsArray()
    @IsPositive({each: true})
    artistsId: number[];

    @IsNumber()
    @IsPositive({each: true})
    userRolesId: number[];
}