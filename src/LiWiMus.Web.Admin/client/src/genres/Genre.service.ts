import { Genre } from "./types/Genre";
import axios from "../shared/services/Axios";
import { UpdateGenreDto } from "./types/UpdateGenreDto";

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
};

export default GenreService;
