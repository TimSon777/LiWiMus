export type Playlist = {
  userId: number;
  userName: string;

  tracksCount: number;
  listenersCount: number;

  id: string;
  name: string;
  isPublic: boolean;
  photoLocation: string | null;
};
