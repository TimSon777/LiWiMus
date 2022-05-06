import {ExternalLogin} from "../../externalLogins/externalLogin.entity";
import {UserArtist} from "../../userArtist/userArtist.entity";
import {IdDto} from "../../shared/dto/id.dto";
import {Equals, IsDate, IsDateString, IsDefined, IsEnum, IsNotEmpty, IsString, MaxLength} from "class-validator";
import {Gender} from "../gender/gender";

export class UpdateUserPersonalDto extends IdDto {
  @MaxLength(50)
  @IsString()
  firstName: string;

  @MaxLength(50)
  @IsString()
  secondName: string;

  @MaxLength(50)
  @IsString()
  patronymic: string;

  @IsEnum(Gender)
  gender: Gender;

  @IsDateString()
  birthDate: Date;
}
