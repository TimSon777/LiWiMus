import { FilterOptions } from "../shared/types/FilterOptions";
import axios from "../shared/services/Axios";
import { PaginatedData } from "../shared/types/PaginatedData";
import { User } from "./types/User";
import { CreateUserDto } from "./types/CreateUserDto";
import FileService from "../shared/services/File.service";
import { UpdateUserDto } from "./types/UpdateUserDto";

const map = (data: any) =>
  new User(
    data.id,
    data.userName,
    data.email,
    data.emailConfirmed,
    data.firstName,
    data.secondName,
    data.patronymic,
    data.birthDate,
    data.gender,
    data.balance,
    data.avatarLocation,
    data.createdAt,
    data.modifiedAt
  );

const UserService = {
  getUsers: async (
    options: FilterOptions<User>
  ): Promise<PaginatedData<User>> => {
    const response = await axios.get(`/users`, {
      params: options,
    });
    const data = response.data;
    return {
      actualPage: data.actualPage,
      itemsPerPage: data.itemsPerPage,
      totalItems: data.totalItems,
      totalPages: data.totalPages,
      hasMore: data.hasMore,
      data: data.data.map((x: any) => map(x)),
    };
  },

  save: async (user: CreateUserDto) => {
    const response = await axios.post("/users", user);
    return map(response.data);
  },

  get: async (id: number | string) => {
    const response = await axios.get(`/users/${id}`);
    return map(response.data);
  },

  update: async (updateDto: UpdateUserDto) => {
    const response = await axios.patch("/users", updateDto);
    return map(response.data);
  },

  removeAvatar: async (user: User) => {
    if (!user.avatarLocation) {
      return user;
    }
    try {
      await FileService.remove(user.avatarLocation);
    } catch (e) {}
    const response = await axios.post(`/users/${user.id}/removeAvatar`);
    return map(response.data);
  },

  changeAvatar: async (user: User, avatar: File) => {
    if (user.avatarLocation) {
      try {
        await FileService.remove(user.avatarLocation);
      } catch (e) {}
    }

    const location = await FileService.save(avatar);
    const updateDto: UpdateUserDto = { id: +user.id, avatarLocation: location };
    return await UserService.update(updateDto);
  },
};

export default UserService;
