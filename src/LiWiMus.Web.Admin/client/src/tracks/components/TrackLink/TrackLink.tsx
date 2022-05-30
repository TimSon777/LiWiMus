import React from "react";
import { Link, Stack } from "@mui/material";
import { Link as RouterLink } from "react-router-dom";
import { Track } from "../../types/Track";
import { useFileService } from "../../../shared/hooks/FileService.hook";

type Props = {
  track: Track;
  cover?: boolean;
};

export default function TrackLink({ track, cover = false }: Props) {
  const fileService = useFileService();

  return (
    <Stack direction={"row"} spacing={2} alignItems={"center"}>
      {cover && track.album && (
        <img
          src={fileService.getLocation(track.album.coverLocation)}
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
