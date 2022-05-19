import axios from "../shared/services/Axios";
import { Album } from "./types/Album";
import FileService from "../shared/services/File.service";
import { UpdateAlbumDto } from "./types/UpdateAlbumDto";

const AlbumService = {
  get: async (id: string | number) =>
    (await axios.get(`/albums/${id}`)).data as Album,

  remove: async (album: Album) => {
    return await axios.delete(`/albums/${album.id}`);
  },

  update: async (album: UpdateAlbumDto) => {
    const response = await axios.patch("/albums", album);
    return response.data as Album;
  },

  changeCover: async (album: Album, cover: File) => {
    if (album.coverLocation) {
      try {
        await FileService.remove(album.coverLocation);
      } catch (e) {
        // @ts-ignore
        console.log(e.message);
      }
    }

    const coverLocation = await FileService.save(cover);
    const updateDto: UpdateAlbumDto = {
      id: album.id,
      coverLocation: coverLocation,
    };
    return await AlbumService.update(updateDto);
  },
};

export default AlbumService;
