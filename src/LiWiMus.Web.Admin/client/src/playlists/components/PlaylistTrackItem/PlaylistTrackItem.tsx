import React, {ReactElement, useEffect, useState} from "react";
import {TableCell, TableRow} from "@mui/material";
import TrackLink from "../../../tracks/components/TrackLink/TrackLink";
import {Track} from "../../../tracks/types/Track";
import AlbumService from "../../../albums/Album.service";
import {useNotifier} from "../../../shared/hooks/Notifier.hook";
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
  const { showError } = useNotifier();
  const [loading, setLoading] = useState<boolean>(true);

  useEffect(() => {
    setLoading(true);
    AlbumService.get(track.albumId)
      .then((album) => (track.album = album))
      .catch(showError)
      .then(() => setLoading(false));
  }, []);

  if (loading) {
    return <></>;
  }

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