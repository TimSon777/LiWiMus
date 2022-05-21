import React from "react";
import { Genre } from "../../../genres/types/Genre";
import DeleteIcon from "@mui/icons-material/Delete";
import { Box, IconButton, Paper, Typography } from "@mui/material";
import styles from "./GenreCard.module.sass";
import { Track } from "../../types/Track";
import TrackService from "../../Track.service";
import { useNotifier } from "../../../shared/hooks/Notifier.hook";

type Props = {
  genre: Genre;
  track: Track;
  setTrack: (track: Track) => void;
};

export default function GenreCard({ genre, track, setTrack }: Props) {
  const { showError, showSuccess } = useNotifier();

  const removeGenre = async () => {
    if (track.genres.length <= 1) {
      showError("The track must have a genre");
      return;
    }

    try {
      await TrackService.removeGenre(track, genre);
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
    <Paper
      elevation={10}
      className={styles.container}
      sx={{ width: "100%", pb: "100%", position: "relative" }}
    >
      <Box className={styles.content}>
        <Typography>{genre.name}</Typography>
      </Box>
      <Box className={styles.actions}>
        <IconButton aria-label="delete" onClick={removeGenre}>
          <DeleteIcon sx={{ fontSize: "2rem" }} />
        </IconButton>
      </Box>
    </Paper>
  );
}
