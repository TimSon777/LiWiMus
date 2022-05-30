import React from "react";
import { Album } from "../../types/Album";
import { useNotifier } from "../../../shared/hooks/Notifier.hook";
import ImageEditor from "../../../shared/components/ImageEditor/ImageEditor";
import { useAlbumService } from "../../AlbumService.hook";

type Props = {
  album: Album;
  coverSrc: string;
  setAlbumWithCover: (album: Album) => void;
};

export default function AlbumImageEditor({
  album,
  coverSrc,
  setAlbumWithCover,
}: Props) {
  const albumService = useAlbumService();
  const { showSuccess, showError } = useNotifier();

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
      const cover = input.files[0];

      const response = await albumService.changeCover(album, cover);
      setAlbumWithCover(response);
      showSuccess("Cover updated");
    } catch (error) {
      // @ts-ignore
      showError(error);
    }
  };

  return (
    <ImageEditor
      width={250}
      src={coverSrc}
      onChange={changePhotoHandler}
      handler1={updatePhotoHandler}
    />
  );
}
