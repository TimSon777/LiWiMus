import React from "react";
import ImageEditor from "../../../shared/components/ImageEditor/ImageEditor";
import { Playlist } from "../../types/Playlist";
import { useSnackbar } from "notistack";
import PlaylistService from "../../Playlist.service";

type Props = {
  playlist: Playlist;
  playlistPhoto: string;
  setPlaylistWithPhoto: (playlist: Playlist) => void;
};

export default function PlaylistImageEditor({
  playlist,
  playlistPhoto,
  setPlaylistWithPhoto,
}: Props) {
  const { enqueueSnackbar } = useSnackbar();

  const updatePhotoHandler = (input: HTMLInputElement) => {
    input.click();
  };

  const removePhotoHandler = async () => {
    if (!playlist.photoLocation) {
      return;
    }
    try {
      const response = await PlaylistService.removePhoto(playlist);
      setPlaylistWithPhoto(response);
      enqueueSnackbar("Photo removed", { variant: "success" });
    } catch (error) {
      // @ts-ignore
      enqueueSnackbar(error.message, { variant: "error" });
    }
  };

  const changePhotoHandler = async (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    const input = event.target;
    if (!input.files || !input.files[0]) {
      return;
    }
    try {
      const photo = input.files[0];

      const response = await PlaylistService.changePhoto(playlist, photo);
      setPlaylistWithPhoto(response);
      enqueueSnackbar("Photo updated", { variant: "success" });
    } catch (error) {
      // @ts-ignore
      enqueueSnackbar(error.message, { variant: "error" });
    }
  };

  return (
    <ImageEditor
      width={250}
      src={playlistPhoto}
      onChange={changePhotoHandler}
      handler1={updatePhotoHandler}
      handler2={removePhotoHandler}
    />
  );
}
