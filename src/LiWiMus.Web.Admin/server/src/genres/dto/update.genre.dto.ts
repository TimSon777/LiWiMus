import {IdDto} from "../../shared/dto/id.dto";
import {ApiProperty} from "@nestjs/swagger";
import {IsString, MaxLength} from "class-validator";
import {Expose} from "class-transformer";

export class UpdateGenreDto extends IdDto {
    @ApiProperty()
    @MaxLength(50)
    @IsString()
    @Expose()
    name: string;
}