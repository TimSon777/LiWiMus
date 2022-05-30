import React from "react";
import ImageEditor from "../../../shared/components/ImageEditor/ImageEditor";
import { Playlist } from "../../types/Playlist";
import { useNotifier } from "../../../shared/hooks/Notifier.hook";
import { usePlaylistService } from "../../PlaylistService.hook";

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
  const playlistService = usePlaylistService();

  const { showSuccess, showError } = useNotifier();

  const updatePhotoHandler = (input: HTMLInputElement) => {
    input.click();
  };

  const removePhotoHandler = async () => {
    if (!playlist.photoLocation) {
      return;
    }
    try {
      const response = await playlistService.removePhoto(playlist);
      setPlaylistWithPhoto(response);
      showSuccess("Photo removed");
    } catch (error) {
      // @ts-ignore
      showError(error);
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

      const response = await playlistService.changePhoto(playlist, photo);
      setPlaylistWithPhoto(response);
      showSuccess("Photo updated");
    } catch (error) {
      // @ts-ignore
      showError(error);
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
