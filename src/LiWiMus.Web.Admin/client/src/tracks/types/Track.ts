import { Artist } from "../../artists/Artist";
import { Album } from "../../albums/Album";

export type Track = {
  id: string;
  name: string;
  publishedAt: string;
  fileLocation: string;
  duration: number;
  artists: Artist[];
  album: Album;
  albumId: string;

  createdAt: Date;
  modifiedAt: Date;
};
