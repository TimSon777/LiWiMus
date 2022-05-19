import React from "react";
import { Stack } from "@mui/material";
import { Artist } from "../../Artist";
import ArtistLink from "../ArtistLink/ArtistLink";

type Props = {
  artists: Artist[];
};

export default function ArtistsList({ artists }: Props) {
  return (
    <Stack direction={"row"} spacing={1} divider={<span>&</span>}>
      {artists.map((artist, index) => (
        <ArtistLink key={index} artist={artist} />
      ))}
    </Stack>
  );
}
