import {IdDto} from "../../shared/dto/id.dto";
import {
    IsArray,
    IsBoolean,
    IsDateString,
    IsEmail,
    IsEnum,
    IsPositive,
    IsString,
    MaxLength
} from "class-validator";
import {Gender} from "../gender/gender";
import {Exclude, Expose} from "class-transformer";
import {Playlist} from "../../playlists/playlist.entity";
import {Transaction} from "../../transactions/transaction.entity";
import {UserRole} from "../../userRoles/userRoles.entity";
import {UserArtist} from "../../userArtist/userArtist.entity";


@Exclude()
export class UserDto extends IdDto {
    
    @Expose()
    @MaxLength(50)
    @IsString()
    firstName: string;

    @Expose()
    @MaxLength(50)
    @IsString()
    secondName: string;

    @Expose()
    @MaxLength(50)
    @IsString()
    patronymic: string;

    @Expose()
    @IsEnum(Gender)
    gender: Gender;

    @Expose()
    @IsDateString()
    birthDate: Date;

    @Expose()
    @MaxLength(256)
    @IsString()
    @IsEmail()
    email: string;

    @Expose()
    @IsBoolean()
    emailConfirmed: boolean;

    @Expose()
    @MaxLength(20)
    @IsString()
    userName: string;

    @Expose()
    userRoles: UserRole[];

    @Expose()
    playlists: Playlist[];

    @Expose()
    @IsArray()
    @IsPositive({each: true})
    artistsId: number[];

    @Expose()
    transactions: Transaction[];
    
    @Expose()
    userArtists: UserArtist[];

    @Expose()
    createdAt: Date;

    @Expose()
    modifiedAt: Date;
}


