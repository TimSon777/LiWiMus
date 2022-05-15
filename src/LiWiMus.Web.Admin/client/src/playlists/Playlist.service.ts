import axios from "../shared/services/Axios";
import {Playlist} from "./types/Playlist";
import FileService from "../shared/services/File.service";
import {UpdatePlaylistDto} from "./types/UpdatePlaylistDto";
import {Track} from "../tracks/types/Track";
import {PaginatedData} from "../shared/types/PaginatedData";

const PlaylistService = {
  get: async (id: string | number) =>
    (await axios.get(`/playlists/${id}`)).data as Playlist,

  remove: async (playlist: Playlist) => {
    if (playlist.photoLocation) {
      await axios.delete(playlist.photoLocation);
    }
    return await axios.delete(`/playlists/${playlist.id}`);
  },

  update: async (dto: UpdatePlaylistDto) => {
    const response = await axios.patch("/playlists", dto);
    return response.data as Playlist;
  },

  removePhoto: async (playlist: Playlist) => {
    if (!playlist.photoLocation) {
      return playlist;
    }
    await axios.delete(playlist.photoLocation);
    const response = await axios.post(`/playlists/${playlist.id}/removePhoto`);
    return response.data as Playlist;
  },

  changePhoto: async (playlist: Playlist, photo: File) => {
    if (playlist.photoLocation) {
      await FileService.remove(playlist.photoLocation);
    }

    const photoLocation = await FileService.save(photo);
    const updateDto = { id: playlist.id, photoLocation };
    return await PlaylistService.update(updateDto);
  },

  addTrack: (playlist: Playlist, track: Track) => {
    const req = { playlistId: playlist.id, trackId: track.id };
    return axios.post("/playlists/tracks", req);
  },

  removeTrack: (playlist: Playlist, track: Track) => {
    const req = { playlistId: playlist.id, trackId: track.id };
    return axios.delete("/playlists/tracks", { data: req });
  },

  getTracks: async (
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

    const response = await axios.get(`/playlists/${playlist.id}/tracks`, {
      params: {
        page,
        itemsPerPage,
      },
    });
    return response.data as PaginatedData<Track>;
  },
};

export default PlaylistService;