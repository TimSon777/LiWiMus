import {IdDto} from "../../shared/dto/id.dto";
import {Column, ManyToMany, OneToMany} from "typeorm";
import {Track} from "../../tracks/track.entity";
import {Artist} from "../../artists/artist.entity";
import {TrackDto} from "../../tracks/dto/track.dto";
import {IsDateString, IsNotEmpty, IsString} from "class-validator";
import {Exclude, Expose} from "class-transformer";
import {ApiProperty} from "@nestjs/swagger";

class ArtistDto {
}

@Exclude()
export class AlbumDto extends IdDto {
    @ApiProperty()
    @Expose()
    @IsString()
    @IsNotEmpty()
    title: string;

    @ApiProperty()
    @Expose()
    @IsDateString()
    @IsNotEmpty()
    publishedAt: string;

    @ApiProperty()
    @Expose()
    @IsNotEmpty()
    artists: ArtistDto[];

    @ApiProperty()
    @Expose()
    coverLocation: string;

    @ApiProperty()
    @Expose()
    createdAt: Date;

    @ApiProperty()
    @Expose()
    modifiedAt: Date;
}