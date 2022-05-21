import { FilterOptions } from "../shared/types/FilterOptions";
import { Artist } from "./types/Artist";
import axios from "../shared/services/Axios";
import { PaginatedData } from "../shared/types/PaginatedData";
import { CreateArtistDto } from "./types/CreateArtistDto";
import FileService from "../shared/services/File.service";
import { UpdateArtistDto } from "./types/UpdateArtistDto";
import { User } from "../users/types/User";

const ArtistService = {
  get: async (id: number | string) => {
    const response = await axios.get(`/artists/${id}`);
    return response.data as Artist;
  },

  getArtists: async (options: FilterOptions<Artist>) => {
    const response = await axios.get(`/artists`, {
      params: options,
    });
    return response.data as PaginatedData<Artist>;
  },

  save: async (artist: CreateArtistDto) => {
    const response = await axios.post(`/artists`, artist);
    return response.data as Artist;
  },

  update: async (dto: UpdateArtistDto) => {
    const response = await axios.patch("/artists", dto);
    return response.data as Artist;
  },

  changePhoto: async (artist: Artist, photo: File) => {
    if (artist.photoLocation) {
      try {
        await FileService.remove(artist.photoLocation);
      } catch (e) {
        // @ts-ignore
        console.error(e.message);
      }
    }

    const photoLocation = await FileService.save(photo);
    const updateDto: UpdateArtistDto = { id: +artist.id, photoLocation };
    return await ArtistService.update(updateDto);
  },

  remove: async (artist: Artist) => {
    return await axios.delete(`/artists/${artist.id}`);
  },

  addOwner: async (artist: Artist, user: User) => {
    return axios.post(`/artists/${artist.id}/users`, { userIds: [user.id] });
  },

  removeOwner: async (artist: Artist, user: User) => {
    return axios.delete(`/artists/${artist.id}/users`, {
      data: { userIds: [user.id] },
    });
  },

  getOwners: async (artist: Artist) => {
    const response = await axios.get(`/artists/${artist.id}/users`);
    return response.data as User[];
  },
};

export default ArtistService;
