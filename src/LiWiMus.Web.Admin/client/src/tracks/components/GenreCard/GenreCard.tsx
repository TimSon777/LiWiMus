import React from "react";
import { Genre } from "../../../genres/types/Genre";
import DeleteIcon from "@mui/icons-material/Delete";
import { IconButton } from "@mui/material";
import { Track } from "../../types/Track";
import { useNotifier } from "../../../shared/hooks/Notifier.hook";
import ActionCard from "../../../shared/components/ActionCard/ActionCard";
import { useTrackService } from "../../TrackService.hook";

type Props = {
  genre: Genre;
  track: Track;
  setTrack: (track: Track) => void;
};

export default function GenreCard({ genre, track, setTrack }: Props) {
  const trackService = useTrackService();

  const { showError, showSuccess } = useNotifier();

  const removeGenre = async () => {
    if (track.genres.length <= 1) {
      showError("The track must have a genre");
      return;
    }

    try {
      await trackService.removeGenre(track, genre);
      setTrack({
        ...track,
        genres: track.genres.filter((t) => t.id !== genre.id),
      });
      showSuccess("Genre removed from track");
    } catch (e) {
      showError(e);
    }
  };

  return (
    <ActionCard
      text={genre.name}
      action={
        <IconButton aria-label="delete" onClick={removeGenre}>
          <DeleteIcon sx={{ fontSize: "2rem" }} />
        </IconButton>
      }
    />
  );
}
