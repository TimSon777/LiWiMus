import {IdDto} from "../../shared/dto/id.dto";
import {ApiProperty} from "@nestjs/swagger";
import {Exclude, Expose} from "class-transformer";
import {IsString, MaxLength} from "class-validator";

@Exclude()
export class UpdateArtistDto extends IdDto {
    @ApiProperty()
    @Expose()
    @IsString()
    @MaxLength(50)
    name: string;

    @ApiProperty()
    @Expose()
    @IsString()
    @MaxLength(500)
    about: string;

    @ApiProperty()
    @Expose()
    @IsString()
    photoLocation: string
}