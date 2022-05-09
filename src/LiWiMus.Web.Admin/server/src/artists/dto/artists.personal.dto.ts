import {IdDto} from "../../shared/dto/id.dto";
import {IsString, MaxLength} from "class-validator";

export class ArtistsPersonalDto extends IdDto {
    @IsString()
    @MaxLength(50)
    name: string;

    @IsString()
    @MaxLength(500)
    about: string;
}