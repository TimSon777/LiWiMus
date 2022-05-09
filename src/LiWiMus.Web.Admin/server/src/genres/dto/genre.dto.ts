import {IdDto} from "../../shared/dto/id.dto";
import {Column, JoinTable, ManyToMany} from "typeorm";
import {Track} from "../../tracks/track.entity";
import {IsArray, IsPositive, IsString, MaxLength} from "class-validator";

export class GenreDto extends IdDto {
    @MaxLength(50)
    @IsString()
    name: string;

    @IsArray()
    @IsPositive({each: true})
    tracksId: number[];
}