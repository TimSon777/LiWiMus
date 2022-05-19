import { Artist } from "../../artists/Artist";

export type Album = {
  id: string;
  title: string;
  publishedAt: string;
  coverLocation: string;

  createdAt: string;
  modifiedAt: string;
  artists: Artist[];

  tracksCount: number;
  listenersCount: number;
};
