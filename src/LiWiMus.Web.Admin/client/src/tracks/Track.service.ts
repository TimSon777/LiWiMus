import axios from "../shared/services/Axios";
import { Artist } from "../artists/Artist";
import { Track } from "./types/Track";
import { PaginatedData } from "../shared/types/PaginatedData";
import { FilterOptions } from "../shared/types/FilterOptions";

const TrackService = {
  getArtists: async (track: Track) =>
    (await axios.get(`/tracks/${track.id}/artists`)).data as Artist[],

  getTracks: async (
    options: FilterOptions<Track>
  ): Promise<PaginatedData<Track>> => {
    const response = await axios.get(`/tracks`, {
      params: options,
    });
    return response.data as PaginatedData<Track>;
  },
};

export default TrackService;
