import { FilterOptions } from "../shared/types/FilterOptions";
import { Artist } from "./types/Artist";
import { PaginatedData } from "../shared/types/PaginatedData";
import { CreateArtistDto } from "./types/CreateArtistDto";
import { UpdateArtistDto } from "./types/UpdateArtistDto";
import { User } from "../users/types/User";
import { useFileService } from "../shared/hooks/FileService.hook";
import { useAxios } from "../shared/hooks/Axios.hook";

export const useArtistService = () => {
  const axios = useAxios("/artists");
  const fileService = useFileService();

  const get = async (id: number | string) => {
    const response = await axios.get(`/${id}`);
    return response.data as Artist;
  };

  const getArtists = async (options: FilterOptions<Artist>) => {
    const response = await axios.get(``, {
      params: options,
    });
    return response.data as PaginatedData<Artist>;
  };

  const save = async (artist: CreateArtistDto) => {
    const response = await axios.post(``, artist);
    return response.data as Artist;
  };

  const update = async (dto: UpdateArtistDto) => {
    const response = await axios.patch("", dto);
    return response.data as Artist;
  };

  const changePhoto = async (artist: Artist, photo: File) => {
    if (artist.photoLocation) {
      try {
        await fileService.remove(artist.photoLocation);
      } catch (e) {
        // @ts-ignore
        console.error(e.message);
      }
    }

    const photoLocation = await fileService.save(photo);
    const updateDto: UpdateArtistDto = { id: +artist.id, photoLocation };
    return await update(updateDto);
  };

  const remove = async (artist: Artist) => {
    return await axios.delete(`/${artist.id}`);
  };

  const addOwner = async (artist: Artist, user: User) => {
    return axios.post(`/${artist.id}/users`, { userIds: [user.id] });
  };

  const removeOwner = async (artist: Artist, user: User) => {
    return axios.delete(`/${artist.id}/users`, {
      data: { userIds: [user.id] },
    });
  };

  const getOwners = async (artist: Artist) => {
    const response = await axios.get(`/${artist.id}/users`);
    return response.data as User[];
  };

  return {
    get,
    update,
    getOwners,
    addOwner,
    getArtists,
    save,
    remove,
    changePhoto,
    removeOwner,
  };
};
