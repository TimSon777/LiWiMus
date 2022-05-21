import axios from "../shared/services/Axios";
import { Artist } from "../artists/types/Artist";
import { Track } from "./types/Track";
import { PaginatedData } from "../shared/types/PaginatedData";
import { FilterOptions } from "../shared/types/FilterOptions";
import { UpdateTrackDto } from "./types/UpdateTrackDto";
import FileService from "../shared/services/File.service";

const getDuration = (file: File) => {
  return new Promise((resolve: (duration: number) => void) => {
    const objectUrl = URL.createObjectURL(file);
    const sound = new Audio(objectUrl);
    sound.addEventListener(
      "canplaythrough",
      () => {
        URL.revokeObjectURL(objectUrl);
        resolve(sound.duration);
      },
      false
    );
  });
};

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

  get: async (id: number | string) => {
    const response = await axios.get(`/tracks/${id}`);
    return response.data as Track;
  },

  remove: async (track: Track) => {
    return await axios.delete(`/tracks/${track.id}`);
  },

  update: async (track: UpdateTrackDto) => {
    const response = await axios.patch("/tracks", track);
    return response.data as Track;
  },

  updateFile: async (track: Track, file: File) => {
    if (track.fileLocation) {
      try {
        await FileService.remove(track.fileLocation);
      } catch (e) {
        console.error(e);
      }
    }

    const duration = await getDuration(file);
    const fileLocation = await FileService.save(file);
    const dto: UpdateTrackDto = { id: +track.id, duration, fileLocation };
    return await TrackService.update(dto);
  },
};

export default TrackService;
