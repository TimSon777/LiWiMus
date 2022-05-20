import { Artist } from "../../artists/types/Artist";
import { Album } from "../../albums/types/Album";

export type Track = {
  id: string;
  name: string;
  publishedAt: string;
  fileLocation: string;
  duration: number;
  artists: Artist[];
  album: Album;

  createdAt: string;
  modifiedAt: string;
};
