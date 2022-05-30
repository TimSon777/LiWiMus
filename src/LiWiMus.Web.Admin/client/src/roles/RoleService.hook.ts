import { Role } from "./types/Role";
import { UpdateRoleDto } from "./types/UpdateRoleDto";
import { SystemPermission } from "../systemPermissions/SystemPermission";
import { useAxios } from "../shared/hooks/Axios.hook";

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

export const useRoleService = () => {
  const axios = useAxios("/roles");

  const get = async (id: number | string) => {
    const response = await axios.get(`/${id}`);
    return map(response.data);
  };

  const remove = async (role: Role) => {
    return await axios.delete(`/${role.id}`);
  };

  const update = async (dto: UpdateRoleDto) => {
    const response = await axios.patch("", dto);
    return map(response.data);
  };

  const replacePermissions = async (
    role: Role,
    permissions: SystemPermission[]
  ) => {
    const dto = {
      roleId: role.id,
      permissions: permissions.map((p) => p.id),
    };
    const response = await axios.put("/systemPermissions", dto);
    return map(response.data);
  };

  const getAll = async (): Promise<Role[]> => {
    const response = await axios.get("");
    return response.data.map((x: any) => map(x));
  };

  return { get, remove, update, replacePermissions, getAll };
};
