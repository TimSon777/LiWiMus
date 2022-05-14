import {IdDto} from "../../shared/dto/id.dto";
import {Column, JoinColumn, ManyToOne, OneToMany} from "typeorm";
import {User} from "../../users/user.entity";
import {PlaylistTrack} from "../../playlistTracks/playlistTrack.entity";
import {Exclude, Expose, Type} from "class-transformer";
import {UserDto} from "../../users/dto/user.dto";
import {IsBoolean, IsNotEmpty, IsString, Length, ValidateNested} from "class-validator";
import {ApiProperty} from "@nestjs/swagger";

@Exclude()
export class PlaylistDto extends IdDto {
    
    @ApiProperty()
    @Expose()
    @IsNotEmpty()
    @ValidateNested()
    @Type(() => UserDto)
    owner: UserDto;

    @ApiProperty()
    @Expose()
    @IsString()
    @Length(50)
    name: string;

    @ApiProperty()
    @Expose()
    @IsBoolean()
    isPublic: boolean;
}