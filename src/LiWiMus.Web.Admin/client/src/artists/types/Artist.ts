import { Album } from "../../albums/types/Album";

export type Artist = {
  id: string;
  name: string;
  about: string;
  photoLocation: string;

  createdAt: string;
  modifiedAt: string;

  tracksCount: number;
  albumsCount: number;

  albums: Album[];
};
