import axios from "../shared/services/Axios";
import { Permission } from "./Permission";

const map = (data: any) =>
  new Permission(
    data.id,
    data.name,
    data.description,
    data.createdAt,
    data.modifiedAt
  );

const PermissionService = {
  getAll: async (): Promise<Permission[]> => {
    const response = await axios.get("/permissions");
    return response.data.map((d: any) => map(d));
  },
};

export default PermissionService;
