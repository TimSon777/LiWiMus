import {IdDto} from "../../shared/dto/id.dto";
import {UserArtist} from "../../userArtist/userArtist.entity";
import {IsArray, IsDefined, IsNotEmpty, IsNumber, IsPositive, ValidateNested} from "class-validator";
import {Type} from "class-transformer";


export class UpdateUserArtistDto extends IdDto {
    @IsArray()
    @IsPositive({each: true})
    artistsId: number[];
} 