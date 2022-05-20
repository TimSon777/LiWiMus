import { FilterOptions } from "../shared/types/FilterOptions";
import { Artist } from "./types/Artist";
import axios from "../shared/services/Axios";
import { PaginatedData } from "../shared/types/PaginatedData";
import { CreateArtistDto } from "./types/CreateArtistDto";

const ArtistService = {
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
};

export default ArtistService;
