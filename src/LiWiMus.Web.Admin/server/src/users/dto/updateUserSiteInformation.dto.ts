import {IdDto} from "../../shared/dto/id.dto";

export class UpdateUserSiteInformationDto extends IdDto{
    email: string;
    emailConfirmed: boolean;
    userName: string;
}