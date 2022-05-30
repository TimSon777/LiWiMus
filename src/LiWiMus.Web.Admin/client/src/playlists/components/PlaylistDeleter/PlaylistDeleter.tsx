import React from "react";
import Deleter from "../../../shared/components/Deleter/Deleter";
import { Playlist } from "../../types/Playlist";
import { useNavigate } from "react-router-dom";
import { useNotifier } from "../../../shared/hooks/Notifier.hook";
import { usePlaylistService } from "../../PlaylistService.hook";
import { Action } from "../../../shared/types/Action";

type Props = {
  playlist: Playlist;
  setPlaylist: Action<Playlist | undefined>;
};

export default function PlaylistDeleter({ playlist, setPlaylist }: Props) {
  const playlistService = usePlaylistService();

  const navigate = useNavigate();
  const { showSuccess, showError } = useNotifier();

  const deleteHandler = async () => {
    try {
      await playlistService.remove(playlist);
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
