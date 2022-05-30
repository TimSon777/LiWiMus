import { Playlist } from "./types/Playlist";
import { UpdatePlaylistDto } from "./types/UpdatePlaylistDto";
import { Track } from "../tracks/types/Track";
import { PaginatedData } from "../shared/types/PaginatedData";
import { FilterOptions } from "../shared/types/FilterOptions";
import { CreatePlaylistDto } from "./types/CreatePlaylistDto";
import { useAxios } from "../shared/hooks/Axios.hook";
import { useFileService } from "../shared/hooks/FileService.hook";

export const usePlaylistService = () => {
  const axios = useAxios("/playlists");
  const fileService = useFileService();

  const get = async (id: string | number) =>
    (await axios.get(`/${id}`)).data as Playlist;

  const getPlaylists = async (
    options: FilterOptions<Playlist>
  ): Promise<PaginatedData<Playlist>> => {
    const response = await axios.get(``, {
      params: options,
    });
    return response.data as PaginatedData<Playlist>;
  };

  const save = async (playlist: CreatePlaylistDto) => {
    const response = await axios.post(``, playlist);
    return response.data as Playlist;
  };

  const remove = async (playlist: Playlist) => {
    if (playlist.photoLocation) {
      await fileService.remove(playlist.photoLocation);
    }
    return await axios.delete(`/${playlist.id}`);
  };

  const update = async (dto: UpdatePlaylistDto) => {
    const response = await axios.patch("", dto);
    return response.data as Playlist;
  };

  const removePhoto = async (playlist: Playlist) => {
    if (!playlist.photoLocation) {
      return playlist;
    }
    try {
      await fileService.remove(playlist.photoLocation);
    } catch (e) {
      console.error(e);
    }
    const response = await axios.post(`/${playlist.id}/removePhoto`);
    return response.data as Playlist;
  };

  const changePhoto = async (playlist: Playlist, photo: File) => {
    if (playlist.photoLocation) {
      try {
        await fileService.remove(playlist.photoLocation);
      } catch (e) {
        console.error(e);
      }
    }

    const photoLocation = await fileService.save(photo);
    const updateDto = { id: playlist.id, photoLocation };
    return await update(updateDto);
  };

  const addTrack = (playlist: Playlist, track: Track) => {
    const req = { playlistId: playlist.id, trackId: track.id };
    return axios.post("/tracks", req);
  };

  const removeTrack = (playlist: Playlist, track: Track) => {
    const req = { playlistId: playlist.id, trackId: track.id };
    return axios.delete("/tracks", { data: req });
  };

  const getTracks = async (
    playlist: Playlist,
    page?: number,
    itemsPerPage?: number
  ) => {
    if (!page) {
      page = 1;
    }
    if (!itemsPerPage) {
      itemsPerPage = 10;
    }

    const response = await axios.get(`/${playlist.id}/tracks`, {
      params: {
        page,
        itemsPerPage,
      },
    });
    return response.data as PaginatedData<Track>;
  };

  return {
    remove,
    update,
    removeTrack,
    getTracks,
    get,
    save,
    changePhoto,
    addTrack,
    getPlaylists,
    removePhoto,
  };
};
