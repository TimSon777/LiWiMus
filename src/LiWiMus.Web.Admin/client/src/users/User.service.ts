import { FilterOptions } from "../shared/types/FilterOptions";
import axios from "../shared/services/Axios";
import { PaginatedData } from "../shared/types/PaginatedData";
import { User } from "./types/User";

const UserService = {
  getUsers: async (options: FilterOptions<User>) => {
    const response = await axios.get(`/users`, {
      params: options,
    });
    return response.data as PaginatedData<User>;
  },
};

export default UserService;
