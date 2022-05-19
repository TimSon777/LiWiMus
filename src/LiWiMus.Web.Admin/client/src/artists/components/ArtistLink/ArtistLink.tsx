import React from "react";
import { Link } from "@mui/material";
import { Link as RouterLink } from "react-router-dom";
import { Artist } from "../../Artist";

type Props = {
  artist: Artist;
};

export default function ArtistLink({ artist }: Props) {
  return (
    <Link
      key={artist.id}
      component={RouterLink}
      to={`/admin/artists/${artist.id}`}
      underline="none"
      color={"secondary"}
    >
      {artist.name}
    </Link>
  );
}
