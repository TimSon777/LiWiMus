export type Playlist = {
  userId: number;
  userName: string;

  id: string;
  name: string;
  isPublic: boolean;
  photoLocation: string | null;
};
