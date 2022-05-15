import React from "react";
import {Grid, Typography} from "@mui/material";
import {Playlist} from "../../types/Playlist";
import TracksInPlaylist from "../TracksInPlaylist/TracksInPlaylist";

type Props = {
  playlist: Playlist;
  setPlaylist: (playlist: Playlist) => void;
};

export default function PlaylistTracks({ playlist, setPlaylist }: Props) {
  return (
    <Grid item xs={12}>
      <Typography variant={"h4"} component={"div"}>
        Tracks
      </Typography>
      <TracksInPlaylist playlist={playlist} setPlaylist={setPlaylist} />
    </Grid>
  );
}