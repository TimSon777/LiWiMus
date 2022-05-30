import { FilterOptions } from "../shared/types/FilterOptions";
import { PaginatedData } from "../shared/types/PaginatedData";
import { User } from "./types/User";
import { CreateUserDto } from "./types/CreateUserDto";
import { UpdateUserDto } from "./types/UpdateUserDto";
import { Role } from "../roles/types/Role";
import { useAxios } from "../shared/hooks/Axios.hook";
import { useFileService } from "../shared/hooks/FileService.hook";

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
    data.modifiedAt,
    data.lockoutEnd
  );

export const useUserService = () => {
  const axios = useAxios("/users");
  const fileService = useFileService();

  const getUsers = async (
    options: FilterOptions<User>
  ): Promise<PaginatedData<User>> => {
    const response = await axios.get(``, {
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
  };

  const save = async (user: CreateUserDto) => {
    const response = await axios.post("", user);
    return map(response.data);
  };

  const get = async (id: number | string) => {
    const response = await axios.get(`/${id}`);
    return map(response.data);
  };

  const update = async (updateDto: UpdateUserDto) => {
    const response = await axios.patch("", updateDto);
    return map(response.data);
  };

  const removeAvatar = async (user: User) => {
    if (!user.avatarLocation) {
      return user;
    }
    try {
      await fileService.remove(user.avatarLocation);
    } catch (e) {}
    const response = await axios.post(`/${user.id}/removeAvatar`);
    return map(response.data);
  };

  const changeAvatar = async (user: User, avatar: File) => {
    if (user.avatarLocation) {
      try {
        await fileService.remove(user.avatarLocation);
      } catch (e) {}
    }

    const location = await fileService.save(avatar);
    const updateDto: UpdateUserDto = { id: +user.id, avatarLocation: location };
    return await update(updateDto);
  };

  const getRoles = async (user: User) => {
    const response = await axios.get(`/${user.id}/roles`);
    return response.data as Role[];
  };

  const addRole = async (user: User, role: Role) => {
    const dto = { userId: user.id, roleId: role.id };
    return await axios.post("/roles", dto);
  };

  const removeRole = async (user: User, role: Role) => {
    const dto = { userId: user.id, roleId: role.id };
    return await axios.delete("/roles", { data: dto });
  };

  const setRandomAvatar = async (user: User) => {
    if (user.avatarLocation) {
      try {
        await fileService.remove(user.avatarLocation);
      } catch (e) {}
    }

    // https://avatars.dicebear.com/api/adventurer/{0}.svg?background=%23EF6817
    const seed = (Math.random() * 1000000000).toFixed(0);
    const url = `https://avatars.dicebear.com/api/adventurer/${seed}.svg?background=%23EF6817`;

    const location = await fileService.saveByUrl(url);
    const updateDto: UpdateUserDto = { id: +user.id, avatarLocation: location };
    return await update(updateDto);
  };

  const lockOut = async (user: User, lockOutEndDate: Date) => {
    const response = await axios.post(`/${user.id}/lockout`, {
      end: lockOutEndDate,
    });
    return map(response.data);
  };

  return {
    lockOut,
    get,
    setRandomAvatar,
    save,
    update,
    addRole,
    changeAvatar,
    getRoles,
    getUsers,
    removeAvatar,
    removeRole,
  };
};
