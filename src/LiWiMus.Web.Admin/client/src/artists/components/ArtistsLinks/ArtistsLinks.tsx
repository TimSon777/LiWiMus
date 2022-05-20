import React from "react";
import { Stack, Typography } from "@mui/material";
import { Artist } from "../../types/Artist";
import ArtistLink from "../ArtistLink/ArtistLink";

const MAX = 2;

type Props = {
  artists: Artist[];
};

export default function ArtistsLinks({ artists }: Props) {
  const truncated = artists.length > MAX;
  if (truncated) {
    artists = artists.slice(0, MAX);
  }

  return (
    <Stack direction={"row"} spacing={1} divider={<span>&</span>}>
      {artists.map((artist, index) => (
        <ArtistLink key={index} artist={artist} />
      ))}
      {truncated && <Typography>...</Typography>}
    </Stack>
  );
}
