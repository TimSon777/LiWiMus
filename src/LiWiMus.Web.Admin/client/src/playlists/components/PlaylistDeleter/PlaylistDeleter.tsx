import React from "react";
import Deleter from "../../../shared/components/Deleter/Deleter";
import { Playlist } from "../../types/Playlist";
import { useNavigate } from "react-router-dom";
import PlaylistService from "../../Playlist.service";
import { useNotifier } from "../../../shared/hooks/Notifier.hook";

type Props = {
  playlist: Playlist;
  setPlaylist: React.Dispatch<React.SetStateAction<Playlist | undefined>>;
};

export default function PlaylistDeleter({ playlist, setPlaylist }: Props) {
  const navigate = useNavigate();
  const { showSuccess, showError } = useNotifier();

  const deleteHandler = async () => {
    try {
      await PlaylistService.remove(playlist);
      setPlaylist(undefined);
      showSuccess("Playlist deleted");

      navigate("/admin/playlists");
    } catch (error) {
      // @ts-ignore
      showError(error);
    }
  };

  return <Deleter itemName={playlist.name} deleteHandler={deleteHandler} />;
}
