import {IdDto} from "../../shared/dto/id.dto";
import {UserRole} from "../../userRoles/userRoles.entity";

export class UpdateUserRoleDto extends IdDto {
    userRoles: UserRole[];
}