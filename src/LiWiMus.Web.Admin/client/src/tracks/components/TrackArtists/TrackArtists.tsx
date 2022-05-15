import React from "react";
import {Link, Stack} from "@mui/material";
import {Link as RouterLink} from "react-router-dom";
import {Track} from "../../types/Track";

type Props = {
  track: Track;
};

export default function TrackArtists({ track }: Props) {
  return (
    <Stack direction={"row"} spacing={1} divider={<span>&</span>}>
      {track.artists.map((artist) => (
        <Link
          key={artist.id}
          component={RouterLink}
          to={`/admin/artists/${artist.id}`}
          underline="none"
          color={"secondary"}
        >
          {artist.name}
        </Link>
      ))}
    </Stack>
  );
}