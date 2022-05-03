import {IdDto} from "../../shared/dto/id.dto";
import {IsBoolean, IsBooleanString, IsDefined, IsEmail, IsNotEmpty, IsString, MaxLength} from "class-validator";

export class UpdateUserSiteInformationDto extends IdDto{
    
    @IsEmail()
    @MaxLength(256)
    @IsString()
    email: string;
    
    @IsBoolean()
    emailConfirmed: boolean;
    
    @MaxLength(20)
    @IsString()
    userName: string;
}

