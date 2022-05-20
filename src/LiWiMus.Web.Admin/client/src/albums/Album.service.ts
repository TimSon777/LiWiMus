import axios from "../shared/services/Axios";
import { Album } from "./types/Album";
import FileService from "../shared/services/File.service";
import { UpdateAlbumDto } from "./types/UpdateAlbumDto";
import { PaginatedData } from "../shared/types/PaginatedData";
import { Artist } from "../artists/types/Artist";

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

  getArtists: async (album: Album, page: number, itemsPerPage: number) => {
    if (!page) {
      page = 1;
    }
    if (!itemsPerPage) {
      itemsPerPage = 10;
    }

    const response = await axios.get(`/albums/${album.id}/artists`, {
      params: {
        page,
        itemsPerPage,
      },
    });
    return response.data as PaginatedData<Artist>;
  },

  removeArtist: async (album: Album, artist: Artist) => {
    const dto = { id: album.id, artistId: artist.id };
    return await axios.delete(`/albums/artists`, { data: dto });
  },

  addArtist: (album: Album, artist: Artist) => {
    const req = { id: album.id, artistId: artist.id };
    return axios.post("/albums/artists", req);
  },
};

export default AlbumService;
