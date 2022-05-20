import React, { ReactElement } from "react";
import { Track } from "../../types/Track";
import {
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
} from "@mui/material";
import TrackLink from "../TrackLink/TrackLink";
import ArtistsLinks from "../../../artists/components/ArtistsLinks/ArtistsLinks";
import { format } from "date-fns";
import AlbumLink from "../../../albums/components/AlbumLink/AlbumLink";
import EditIcon from "@mui/icons-material/Edit";

type Props = {
  tracks: Track[];
  albumCover?: boolean;
  name?: boolean;
  publishedAt?: boolean;
  duration?: boolean;
  artists?: boolean;
  album?: boolean;
  createdAt?: boolean;
  modifiedAt?: boolean;
  action?: (track: Track) => ReactElement;
};

export default function TracksList({
  tracks,
  name = true,
  duration,
  albumCover = true,
  album = true,
  artists = true,
  publishedAt,
  modifiedAt,
  createdAt,
  action,
}: Props) {
  return (
    <TableContainer component={Paper}>
      <Table>
        <TableHead>
          <TableRow>
            <TableCell>#</TableCell>
            {name && <TableCell>Name</TableCell>}
            {album && <TableCell>Album</TableCell>}
            {artists && <TableCell>Artists</TableCell>}
            {publishedAt && <TableCell>Published at</TableCell>}
            {duration && <TableCell>Duration</TableCell>}
            {createdAt && <TableCell>Created at</TableCell>}
            {modifiedAt && <TableCell>Modified at</TableCell>}
            {action && (
              <TableCell>
                <EditIcon />
              </TableCell>
            )}
          </TableRow>
        </TableHead>
        <TableBody>
          {tracks.map((track, index) => (
            <TableRow key={index}>
              <TableCell>{index + 1}</TableCell>
              {name && (
                <TableCell>
                  <TrackLink track={track} cover={albumCover} />
                </TableCell>
              )}
              {album && (
                <TableCell>
                  <AlbumLink album={track.album} />
                </TableCell>
              )}
              {artists && (
                <TableCell>
                  <ArtistsLinks artists={track.artists} />
                </TableCell>
              )}
              {publishedAt && <TableCell>{track.publishedAt}</TableCell>}
              {duration && <TableCell>{track.duration} seconds</TableCell>}
              {createdAt && (
                <TableCell>
                  {format(new Date(track.createdAt), "dd.MM.yyyy HH:mm")}
                </TableCell>
              )}
              {modifiedAt && (
                <TableCell>
                  {format(new Date(track.modifiedAt), "dd.MM.yyyy HH:mm")}
                </TableCell>
              )}
              {action && <TableCell>{action(track)}</TableCell>}
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </TableContainer>
  );
}
