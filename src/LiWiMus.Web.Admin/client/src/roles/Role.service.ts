import axios from "../shared/services/Axios";
import {Role} from "./types/Role";
import {UpdateRoleDto} from "./types/UpdateRoleDto";
import {SystemPermission} from "../systemPermissions/SystemPermission";

const map = (data: any) => {
  return new Role(
    data.id,
    data.name,
    data.description,
    data.pricePerMonth,
    data.permissions,
    data.createdAt,
    data.modifiedAt
  );
};

const RoleService = {
  get: async (id: number | string) => {
    const response = await axios.get(`/roles/${id}`);
    return map(response.data);
  },

  remove: async (role: Role) => {
    return await axios.delete(`/roles/${role.id}`);
  },

  update: async (dto: UpdateRoleDto) => {
    const response = await axios.patch("/roles", dto);
    return map(response.data);
  },

  replacePermissions: async (role: Role, permissions: SystemPermission[]) => {
    const dto = {
      roleId: role.id,
      permissions: permissions.map((p) => p.id),
    };
    const response = await axios.put("/roles/systemPermissions", dto);
    return map(response.data);
  },
};

export default RoleService;