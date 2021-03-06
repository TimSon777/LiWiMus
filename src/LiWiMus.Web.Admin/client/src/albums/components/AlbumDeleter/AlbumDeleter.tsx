import React from "react";
import { Album } from "../../types/Album";
import { useNavigate } from "react-router-dom";
import { useNotifier } from "../../../shared/hooks/Notifier.hook";
import Deleter from "../../../shared/components/Deleter/Deleter";
import { useAlbumService } from "../../AlbumService.hook";

type Props = {
  album: Album;
  setAlbum: React.Dispatch<React.SetStateAction<Album | undefined>>;
};

export default function AlbumDeleter({ album, setAlbum }: Props) {
  const navigate = useNavigate();
  const { showSuccess, showError } = useNotifier();
  const albumService = useAlbumService();

  const deleteHandler = async () => {
    try {
      await albumService.remove(album);
      setAlbum(undefined);
      showSuccess("Album deleted");

      navigate("/admin/albums");
    } catch (error) {
      // @ts-ignore
      showError(error);
    }
  };

  return <Deleter itemName={album.title} deleteHandler={deleteHandler} />;
}
