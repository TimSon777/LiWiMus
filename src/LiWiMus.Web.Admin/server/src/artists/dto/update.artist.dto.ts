import {IdDto} from "../../shared/dto/id.dto";
import {IsArray, IsPositive, IsString, MaxLength} from "class-validator";
import {Expose} from "class-transformer";

export class UpdateArtistDto extends IdDto {
    @Expose()
    @IsString()
    @MaxLength(50)
    name: string;

    @Expose()
    @IsString()
    @MaxLength(500)
    about: string;
    
    @IsArray()
    @IsPositive({each: true})
    userArtistsId: number[];
}