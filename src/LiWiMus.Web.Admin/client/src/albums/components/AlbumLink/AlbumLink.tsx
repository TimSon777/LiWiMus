import React from "react";
import { Link } from "@mui/material";
import { Link as RouterLink } from "react-router-dom";
import { Album } from "../../types/Album";

type Props = {
  album: Album;
};

export default function AlbumLink({ album }: Props) {
  if (!album) {
    return <></>;
  }

  return (
    <Link
      component={RouterLink}
      to={`/admin/albums/${album.id}`}
      underline="none"
      color={"secondary"}
    >
      {album.title}
    </Link>
  );
}
