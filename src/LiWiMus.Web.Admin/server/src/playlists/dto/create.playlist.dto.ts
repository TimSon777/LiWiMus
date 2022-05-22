import {IdDto} from "../../shared/dto/id.dto";
import {Column, JoinColumn, ManyToOne, OneToMany} from "typeorm";
import {User} from "../../users/user.entity";
import {PlaylistTrack} from "../../playlistTracks/playlistTrack.entity";
import {IsArray, IsBoolean, IsInt, IsNotEmpty, IsString, MaxLength} from "class-validator";
import {Exclude, Expose, Type} from "class-transformer";
import {ApiProperty} from "@nestjs/swagger";

@Exclude()
export class CreatePlaylistDto {
    @ApiProperty()
    @Expose()
    @IsNotEmpty()
    @IsInt()
    owner: number;

    @ApiProperty()
    @Expose()
    @IsNotEmpty()
    @IsString()
    @MaxLength(50)
    name: string;

    @ApiProperty()
    @Expose()
    @IsNotEmpty()
    @IsBoolean()
    isPublic: boolean;

    @ApiProperty()
    @Expose()
    @IsNotEmpty()
    @IsString()
    photoLocation: string;
}