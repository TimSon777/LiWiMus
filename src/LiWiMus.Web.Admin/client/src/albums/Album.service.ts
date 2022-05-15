import axios from "../shared/services/Axios";
import {Album} from "./Album";

const AlbumService = {
  get: async (id: string | number) =>
    (await axios.get(`/albums/${id}`)).data as Album,
};

export default AlbumService;