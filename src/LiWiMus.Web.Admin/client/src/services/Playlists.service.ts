import { Playlist } from "../types/Playlist";
import { HttpMethod } from "../types/HttpMethod";

const API_URL = process.env.REACT_APP_API_URL;

type processProps = {
  urlPostfix: string;
  method?: HttpMethod;
  body?: BodyInit | null;
};

const processPlaylist = async ({
  urlPostfix,
  method = "GET",
  body,
}: processProps): Promise<Playlist> => {
  const res = await fetch(`${API_URL}/playlists${urlPostfix}`, {
    method,
    body,
  });
  if (!res.ok) {
    throw res.statusText;
  }
  return (await res.json()) as Playlist;
};

const PlaylistsService = {
  getById: async (id: string): Promise<Playlist | null> =>
    await processPlaylist({ urlPostfix: `/${id}` }),

  update: async (data: FormData): Promise<Playlist | null> =>
    await processPlaylist({
      urlPostfix: "",
      method: "PATCH",
      body: data,
    }),

  removePhoto: async (id: string): Promise<Playlist | null> =>
    await processPlaylist({ urlPostfix: `/${id}/removePhoto`, method: "POST" }),
};

export default PlaylistsService;
