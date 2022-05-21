import React from "react";
import { Artist } from "../../types/Artist";
import { useNotifier } from "../../../shared/hooks/Notifier.hook";
import ImageEditor from "../../../shared/components/ImageEditor/ImageEditor";
import ArtistService from "../../Artist.service";
import FileService from "../../../shared/services/File.service";

type Props = {
  artist: Artist;
  setArtist: (artist: Artist) => void;
};

export default function ArtistImageEditor({ artist, setArtist }: Props) {
  const { showSuccess, showError } = useNotifier();
  const photoSrc = FileService.getLocation(artist.photoLocation);

  const updatePhotoHandler = (input: HTMLInputElement) => {
    input.click();
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

      const response = await ArtistService.changePhoto(artist, photo);
      setArtist(response);
      showSuccess("Photo updated");
    } catch (error) {
      // @ts-ignore
      showError(error);
    }
  };

  return (
    <ImageEditor
      width={250}
      src={photoSrc}
      onChange={changePhotoHandler}
      handler1={updatePhotoHandler}
    />
  );
}
