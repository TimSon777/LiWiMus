import { Permission } from "./Permission";
import { useAxios } from "../shared/hooks/Axios.hook";

const map = (data: any) =>
  new Permission(
    data.id,
    data.name,
    data.description,
    data.createdAt,
    data.modifiedAt
  );

export const usePermissionService = () => {
  const axios = useAxios("/permissions");

  const getAll = async (): Promise<Permission[]> => {
    const response = await axios.get("");
    return response.data.map((d: any) => map(d));
  };

  return { getAll };
};
