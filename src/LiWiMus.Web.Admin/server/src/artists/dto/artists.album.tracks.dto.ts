import {IdDto} from "../../shared/dto/id.dto";
import {IsArray, IsPositive} from "class-validator";

export class ArtistsAlbumTracksDto extends IdDto {
    @IsArray()
    @IsPositive({each: true})
    albumsId: number[];

    @IsArray()
    @IsPositive({each: true})
    tracksId: number[];
}