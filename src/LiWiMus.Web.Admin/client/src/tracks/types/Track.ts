import { Artist } from "../../artists/types/Artist";
import { Album } from "../../albums/types/Album";
import { Genre } from "../../genres/types/Genre";

export type Track = {
  id: string;
  name: string;
  publishedAt: string;
  fileLocation: string;
  duration: number;
  artists: Artist[];
  album: Album;

  genres: Genre[];

  createdAt: string;
  modifiedAt: string;
};
