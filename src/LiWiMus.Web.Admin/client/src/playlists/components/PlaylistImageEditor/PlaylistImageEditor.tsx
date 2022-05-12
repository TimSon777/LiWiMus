import React from "react";
import ImageEditor from "../../../shared/components/ImageEditor/ImageEditor";
import axios from "../../../shared/services/Axios";
import { Playlist } from "../../types/Playlist";
import { useSnackbar } from "notistack";

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
      await axios.delete(playlist.photoLocation);
      const response = await axios.post(
        `/playlists/${playlist.id}/removePhoto`
      );
      setPlaylistWithPhoto(response.data as Playlist);
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

      if (playlist.photoLocation) {
        await axios.delete(playlist.photoLocation);
      }

      const formData = new FormData();
      formData.append("file", photo);
      const { data } = await axios.post("/files", formData, {
        headers: { "Content-Type": "multipart/form-data" },
      });
      const photoLocation = data.location as string;
      const updateDto = { id: playlist.id, photoLocation };
      const response = await axios.patch("/playlists", updateDto);
      setPlaylistWithPhoto(response.data as Playlist);
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
