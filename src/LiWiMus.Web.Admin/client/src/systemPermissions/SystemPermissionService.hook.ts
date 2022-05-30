import { SystemPermission } from "./SystemPermission";
import { useAxios } from "../shared/hooks/Axios.hook";

const map = (data: any) =>
  new SystemPermission(
    data.id,
    data.name,
    data.description,
    data.createdAt,
    data.modifiedAt
  );

export const useSystemPermissionService = () => {
  const axios = useAxios("/systemPermissions");

  const getAll = async (): Promise<SystemPermission[]> => {
    const response = await axios.get("");
    return response.data.map((d: any) => map(d));
  };

  return { getAll };
};
