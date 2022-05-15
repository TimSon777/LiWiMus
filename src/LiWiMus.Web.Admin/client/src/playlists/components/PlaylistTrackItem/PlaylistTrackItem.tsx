import React, {ReactElement} from "react";
import {TableCell, TableRow} from "@mui/material";
import TrackLink from "../../../tracks/components/TrackLink/TrackLink";
import {Track} from "../../../tracks/types/Track";
import TrackArtists from "../../../tracks/components/TrackArtists/TrackArtists";
import AlbumLink from "../../../albums/components/AlbumLink";

type Props = {
  track: Track;
  index: number;
  renderAction: () => ReactElement;
};

export default function PlaylistTrackItem({
  track,
  index,
  renderAction,
}: Props) {
  const action = renderAction();

  return (
    <TableRow>
      <TableCell>{index + 1}</TableCell>
      <TableCell>
        <TrackLink track={track} cover />
      </TableCell>
      <TableCell>
        <TrackArtists track={track} />
      </TableCell>
      <TableCell>
        <AlbumLink album={track.album} />
      </TableCell>
      <TableCell>{action}</TableCell>
    </TableRow>
  );
}