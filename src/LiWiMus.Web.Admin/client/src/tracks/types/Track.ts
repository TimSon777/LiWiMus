import { Artist } from "../../artists/Artist";
import { Album } from "../../albums/types/Album";

export type Track = {
  id: string;
  name: string;
  publishedAt: Date;
  fileLocation: string;
  duration: number;
  artists: Artist[];
  album: Album;

  createdAt: Date;
  modifiedAt: Date;
};
