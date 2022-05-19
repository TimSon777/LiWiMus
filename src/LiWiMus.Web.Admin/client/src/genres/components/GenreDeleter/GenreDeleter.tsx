import React from "react";
import { Genre } from "../../types/Genre";
import { useNavigate } from "react-router-dom";
import { useNotifier } from "../../../shared/hooks/Notifier.hook";
import Deleter from "../../../shared/components/Deleter/Deleter";
import GenreService from "../../Genre.service";

type Props = {
  genre: Genre;
  setGenre: (genre: Genre | undefined) => void;
};

export default function GenreDeleter({ genre, setGenre }: Props) {
  const navigate = useNavigate();
  const { showSuccess, showError } = useNotifier();

  const deleteHandler = async () => {
    try {
      await GenreService.remove(genre);
      setGenre(undefined);
      showSuccess("Genre deleted");

      navigate("/admin/genres");
    } catch (error) {
      // @ts-ignore
      showError(error);
    }
  };

  return <Deleter itemName={genre.name} deleteHandler={deleteHandler} />;
}
