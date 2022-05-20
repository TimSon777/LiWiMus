import {IdDto} from "../../shared/dto/id.dto";
import {ApiProperty} from "@nestjs/swagger";
import {Expose} from "class-transformer";
import {IsString, MaxLength} from "class-validator";

export class UpdateArtistDto extends IdDto {
    @ApiProperty({required: false, nullable: true})
    @Expose()
    @IsString()
    @MaxLength(50)
    name: string;

    @ApiProperty({required: false, nullable: true})
    @Expose()
    @IsString()
    @MaxLength(500)
    about: string;

    @ApiProperty({required: false, nullable: true})
    @Expose()
    @IsString()
    photoLocation: string
}