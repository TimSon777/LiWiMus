import { Album } from "./types/Album";
import { UpdateAlbumDto } from "./types/UpdateAlbumDto";
import { PaginatedData } from "../shared/types/PaginatedData";
import { Artist } from "../artists/types/Artist";
import { FilterOptions } from "../shared/types/FilterOptions";
import { CreateAlbumDto } from "./types/CreateAlbumDto";
import { useAxios } from "../shared/hooks/Axios.hook";
import { useFileService } from "../shared/hooks/FileService.hook";

export const useAlbumService = () => {
  const axios = useAxios("/albums");
  const fileService = useFileService();

  const get = async (id: string | number) =>
    (await axios.get(`/${id}`)).data as Album;

  const remove = async (album: Album) => {
    return await axios.delete(`/${album.id}`);
  };

  const update = async (album: UpdateAlbumDto) => {
    const response = await axios.patch("", album);
    return response.data as Album;
  };

  const getAlbums = async (options: FilterOptions<Album>) => {
    const response = await axios.get(``, {
      params: options,
    });
    return response.data as PaginatedData<Album>;
  };

  const save = async (artist: CreateAlbumDto) => {
    const response = await axios.post(``, artist);
    return response.data as Album;
  };

  const changeCover = async (album: Album, cover: File) => {
    if (album.coverLocation) {
      try {
        await fileService.remove(album.coverLocation);
      } catch (e) {
        // @ts-ignore
        console.error(e.message);
      }
    }

    const coverLocation = await fileService.save(cover);
    const updateDto: UpdateAlbumDto = {
      id: album.id,
      coverLocation: coverLocation,
    };
    return await update(updateDto);
  };

  const getArtists = async (
    album: Album,
    page: number,
    itemsPerPage: number
  ) => {
    if (!page) {
      page = 1;
    }
    if (!itemsPerPage) {
      itemsPerPage = 10;
    }

    const response = await axios.get(`/${album.id}/artists`, {
      params: {
        page,
        itemsPerPage,
      },
    });
    return response.data as PaginatedData<Artist>;
  };

  const removeArtist = async (album: Album, artist: Artist) => {
    const dto = { id: album.id, artistId: artist.id };
    return await axios.delete(`/artists`, { data: dto });
  };

  const addArtist = (album: Album, artist: Artist) => {
    const req = { id: album.id, artistId: artist.id };
    return axios.post("/artists", req);
  };

  return {
    remove,
    update,
    addArtist,
    save,
    getArtists,
    changeCover,
    get,
    removeArtist,
    getAlbums,
  };
};
