import {IdDto} from "../../shared/dto/id.dto";
import {IsArray, IsPositive} from "class-validator";

export class UserArtistDto extends IdDto {
    @IsArray()
    @IsPositive({each: true})
    userArtistsId: number[];
}