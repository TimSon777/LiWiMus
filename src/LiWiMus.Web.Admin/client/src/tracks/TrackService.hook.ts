import { Artist } from "../artists/types/Artist";
import { Track } from "./types/Track";
import { PaginatedData } from "../shared/types/PaginatedData";
import { FilterOptions } from "../shared/types/FilterOptions";
import { UpdateTrackDto } from "./types/UpdateTrackDto";
import { CreateTrackDto } from "./types/CreateTrackDto";
import { Genre } from "../genres/types/Genre";
import { useAxios } from "../shared/hooks/Axios.hook";
import { useFileService } from "../shared/hooks/FileService.hook";

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

export const useTrackService = () => {
  const axios = useAxios("/tracks");
  const fileService = useFileService();

  const getArtists = async (track: Track) =>
    (await axios.get(`/${track.id}/artists`)).data as Artist[];

  const getTracks = async (
    options: FilterOptions<Track>
  ): Promise<PaginatedData<Track>> => {
    const response = await axios.get(``, {
      params: options,
    });
    return response.data as PaginatedData<Track>;
  };

  const get = async (id: number | string) => {
    const response = await axios.get(`/${id}`);
    return response.data as Track;
  };

  const save = async (track: CreateTrackDto) => {
    const response = await axios.post(``, track);
    return response.data as Artist;
  };

  const remove = async (track: Track) => {
    return await axios.delete(`/${track.id}`);
  };

  const update = async (track: UpdateTrackDto) => {
    const response = await axios.patch("", track);
    return response.data as Track;
  };

  const updateFile = async (track: Track, file: File) => {
    const audio = await getAudio(file);

    if (track.fileLocation) {
      try {
        await fileService.remove(track.fileLocation);
      } catch (e) {
        console.error(e);
      }
    }

    const fileLocation = await fileService.save(file);
    const dto: UpdateTrackDto = {
      id: +track.id,
      duration: audio.duration,
      fileLocation,
    };
    return await update(dto);
  };

  const addGenre = async (track: Track, genre: Genre) => {
    const dto = { genreId: +genre.id };
    return await axios.post(`/${track.id}/genres`, dto);
  };

  const removeGenre = async (track: Track, genre: Genre) => {
    const dto = { genreId: +genre.id };
    return await axios.delete(`/${track.id}/genres`, { data: dto });
  };

  const addArtist = async (track: Track, artist: Artist) => {
    const dto = { artistId: +artist.id };
    return await axios.post(`/${track.id}/artists`, dto);
  };

  const removeArtist = async (track: Track, artist: Artist) => {
    const dto = { artistId: +artist.id };
    return await axios.delete(`/${track.id}/artists`, { data: dto });
  };

  return {
    save,
    addArtist,
    removeArtist,
    removeGenre,
    addGenre,
    get,
    getTracks,
    remove,
    update,
    updateFile,
    getArtists,
  };
};
