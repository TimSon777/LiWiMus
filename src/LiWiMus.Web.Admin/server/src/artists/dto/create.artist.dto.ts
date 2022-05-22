import {IdDto} from "../../shared/dto/id.dto";
import {ApiProperty} from "@nestjs/swagger";
import {Exclude, Expose} from "class-transformer";
import {IsArray, IsInt, IsNotEmpty, IsString, MaxLength} from "class-validator";
import {UserArtist} from "../../userArtist/userArtist.entity";
import {Album} from "../../albums/album.entity";
import {Track} from "../../tracks/track.entity";


@Exclude()
export class CreateArtistDto {
    @ApiProperty()
    @Expose()
    @IsNotEmpty()
    @IsString()
    @MaxLength(50)
    name: string;

    @ApiProperty()
    @Expose()
    @IsNotEmpty()
    @IsString()
    @MaxLength(500)
    about: string;

    @ApiProperty()
    @Expose()
    @IsNotEmpty()
    @IsString()
    photoLocation: string;
}