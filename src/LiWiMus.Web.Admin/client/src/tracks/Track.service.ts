import axios from "../shared/services/Axios";
import {Artist} from "../artists/Artist";
import {Track} from "./types/Track";

const TrackService = {
  getArtists: async (track: Track) =>
    (await axios.get(`/tracks/${track.id}/artists`)).data as Artist[],
};

export default TrackService;