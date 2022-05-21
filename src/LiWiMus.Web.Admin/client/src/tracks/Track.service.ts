import axios from "../shared/services/Axios";
import { Artist } from "../artists/types/Artist";
import { Track } from "./types/Track";
import { PaginatedData } from "../shared/types/PaginatedData";
import { FilterOptions } from "../shared/types/FilterOptions";
import { UpdateTrackDto } from "./types/UpdateTrackDto";
import FileService from "../shared/services/File.service";
import { Genre } from "../genres/types/Genre";

const getAudio = (file: File): Promise<HTMLAudioElement> => {
  return new Promise((resolve, reject) => {
    const url = URL.createObjectURL(file);
    const sound = new Audio(url);
    sound.addEventListener("error", () => {
      reject("Bad audio file");
    });
    sound.addEventListener("canplaythrough", () => {
      URL.revokeObjectURL(url);
      resolve(sound);
    });
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
    const audio = await getAudio(file);

    if (track.fileLocation) {
      await FileService.remove(track.fileLocation);
    }

    const fileLocation = await FileService.save(file);
    const dto: UpdateTrackDto = {
      id: +track.id,
      duration: audio.duration,
      fileLocation,
    };
    return await TrackService.update(dto);
  },

  addGenre: async (track: Track, genre: Genre) => {
    const dto = { genreId: +genre.id };
    return await axios.post(`tracks/${track.id}/genres`, dto);
  },

  removeGenre: async (track: Track, genre: Genre) => {
    const dto = { genreId: +genre.id };
    return await axios.delete(`tracks/${track.id}/genres`, { data: dto });
  },
};

export default TrackService;
