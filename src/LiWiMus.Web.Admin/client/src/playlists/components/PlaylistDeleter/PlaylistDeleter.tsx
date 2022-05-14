import React from "react";
import Deleter from "../../../shared/components/Deleter/Deleter";
import { Playlist } from "../../types/Playlist";
import { useNavigate } from "react-router-dom";
import { useSnackbar } from "notistack";
import PlaylistService from "../../Playlist.service";

type Props = {
  playlist: Playlist;
  setPlaylist: React.Dispatch<React.SetStateAction<Playlist | undefined>>;
};

export default function PlaylistDeleter({ playlist, setPlaylist }: Props) {
  const navigate = useNavigate();
  const { enqueueSnackbar } = useSnackbar();

  const deleteHandler = async () => {
    try {
      await PlaylistService.remove(playlist);
      setPlaylist(undefined);
      enqueueSnackbar("Playlist deleted", { variant: "success" });

      navigate("/admin/playlists");
    } catch (error) {
      // @ts-ignore
      enqueueSnackbar(error.message, { variant: "error" });
    }
  };

  return <Deleter itemName={playlist.name} deleteHandler={deleteHandler} />;
}
