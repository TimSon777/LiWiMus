import React from "react";
import { Artist } from "../../types/Artist";
import { useNotifier } from "../../../shared/hooks/Notifier.hook";
import ImageEditor from "../../../shared/components/ImageEditor/ImageEditor";
import { useArtistService } from "../../ArtistService.hook";
import { useFileService } from "../../../shared/hooks/FileService.hook";

type Props = {
  artist: Artist;
  setArtist: (artist: Artist) => void;
};

export default function ArtistImageEditor({ artist, setArtist }: Props) {
  const artistService = useArtistService();
  const fileService = useFileService();

  const { showSuccess, showError } = useNotifier();
  const photoSrc = fileService.getLocation(artist.photoLocation);

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

      const response = await artistService.changePhoto(artist, photo);
      setArtist({ ...artist, ...response });
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
