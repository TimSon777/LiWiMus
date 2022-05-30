import { Genre } from "./types/Genre";
import { UpdateGenreDto } from "./types/UpdateGenreDto";
import { CreateGenreDto } from "./types/CreateGenreDto";
import { FilterOptions } from "../shared/types/FilterOptions";
import { PaginatedData } from "../shared/types/PaginatedData";
import { useAxios } from "../shared/hooks/Axios.hook";

export const useGenreService = () => {
  const axios = useAxios("/genres");

  const get = async (id: string): Promise<Genre> =>
    (await axios.get(`/${id}`)).data as Genre;

  const remove = async (genre: Genre) => {
    return await axios.delete(`/${genre.id}`);
  };

  const update = async (dto: UpdateGenreDto) => {
    const response = await axios.patch("", dto);
    return response.data as Genre;
  };

  const save = async (genre: CreateGenreDto) => {
    const response = await axios.post("", genre);
    return response.data as Genre;
  };

  const getGenres = async (
    options: FilterOptions<Genre>
  ): Promise<PaginatedData<Genre>> => {
    const response = await axios.get(``, {
      params: options,
    });
    return response.data as PaginatedData<Genre>;
  };

  return { get, remove, update, save, getGenres };
};
