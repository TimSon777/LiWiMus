import React from "react";
import {Link, Stack} from "@mui/material";
import {Link as RouterLink} from "react-router-dom";
import {Track} from "../../types/Track";

type Props = {
  track: Track;
  cover?: boolean;
};

const API_URL = process.env.REACT_APP_API_URL;

export default function TrackLink({ track, cover = false }: Props) {
  return (
    <Stack direction={"row"} spacing={2} alignItems={"center"}>
      {cover && track.album && (
        <img
          src={API_URL + track.album.coverLocation}
          alt={track.name}
          width={40}
          height={40}
        />
      )}
      <Link
        component={RouterLink}
        to={`/admin/tracks/${track.id}`}
        underline="none"
        color={"secondary"}
      >
        {track.name}
      </Link>
    </Stack>
  );
}