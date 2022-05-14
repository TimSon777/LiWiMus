export type UpdatePlaylistDto = {
  id: string;
  name?: string;
  isPublic?: boolean;
  photoLocation?: string | null;
};
