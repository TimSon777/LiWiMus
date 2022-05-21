import React from "react";
import { Artist } from "../../types/Artist";
import { useNavigate } from "react-router-dom";
import { useNotifier } from "../../../shared/hooks/Notifier.hook";
import Deleter from "../../../shared/components/Deleter/Deleter";
import ArtistService from "../../Artist.service";

type Props = {
  artist: Artist;
  setArtist: (artist: Artist | undefined) => void;
};

export default function ArtistDeleter({ artist, setArtist }: Props) {
  const navigate = useNavigate();
  const { showSuccess, showError } = useNotifier();

  const deleteHandler = async () => {
    try {
      await ArtistService.remove(artist);
      setArtist(undefined);
      showSuccess("Artist deleted");

      navigate("/admin/artists");
    } catch (error) {
      // @ts-ignore
      showError(error);
    }
  };

  return <Deleter itemName={artist.name} deleteHandler={deleteHandler} />;
}
