import { FilterOptions } from "../shared/types/FilterOptions";
import axios from "../shared/services/Axios";
import { PaginatedData } from "../shared/types/PaginatedData";
import { User } from "./types/User";
import {CreateUserDto} from "./types/CreateUserDto"

const UserService = {
  getUsers: async (options: FilterOptions<User>) => {
    const response = await axios.get(`/users`, {
      params: options,
    });
    return response.data as PaginatedData<User>;
  },
  save: async (user: CreateUserDto) => {
    const response = await axios.post('/users', user);
    return response.data as User;
  }
};

export default UserService;
