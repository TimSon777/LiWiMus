import { Genre } from "./types/Genre";
import axios from "../shared/services/Axios";
import { UpdateGenreDto } from "./types/UpdateGenreDto";
import { CreateGenreDto } from "./types/CreateGenreDto"
import { FilterOptions } from "../shared/types/FilterOptions";
import { PaginatedData } from "../shared/types/PaginatedData";

const GenreService = {
  get: async (id: string): Promise<Genre> =>
    (await axios.get(`/genres/${id}`)).data as Genre,

  remove: async (genre: Genre) => {
    return await axios.delete(`/genres/${genre.id}`);
  },

  update: async (dto: UpdateGenreDto) => {
    const response = await axios.patch("/genres", dto);
    return response.data as Genre;
  },
  
  save: async (genre: CreateGenreDto) => {
    const response = await axios.post("/genres", genre);
    return response.data as Genre
  },

  getGenres: async (
    options: FilterOptions<Genre>
  ): Promise<PaginatedData<Genre>> => {
    const response = await axios.get(`/genres`, {
      params: options,
    });
    return response.data as PaginatedData<Genre>;
  },
};

export default GenreService;
