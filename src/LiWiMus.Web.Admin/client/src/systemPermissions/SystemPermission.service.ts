import axios from "../shared/services/Axios";
import {SystemPermission} from "./SystemPermission";

const map = (data: any) =>
  new SystemPermission(
    data.id,
    data.name,
    data.description,
    data.createdAt,
    data.modifiedAt
  );

const SystemPermissionService = {
  getAll: async (): Promise<SystemPermission[]> => {
    const response = await axios.get("/systemPermissions");
    return response.data.map((d: any) => map(d));
  },
};

export default SystemPermissionService;