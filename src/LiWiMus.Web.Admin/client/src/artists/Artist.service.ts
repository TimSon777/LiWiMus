import { FilterOptions } from "../shared/types/FilterOptions";
import { Artist } from "./types/Artist";
import axios from "../shared/services/Axios";
import { PaginatedData } from "../shared/types/PaginatedData";

const ArtistService = {
  getArtists: async (options: FilterOptions<Artist>) => {
    const response = await axios.get(`/artists`, {
      params: options,
    });
    return response.data as PaginatedData<Artist>;
  },
};

export default ArtistService;
