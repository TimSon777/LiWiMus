import React, { ReactElement } from "react";
import { Artist } from "../../types/Artist";
import {
  Avatar,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
} from "@mui/material";
import EditIcon from "@mui/icons-material/Edit";
import { format } from "date-fns";
import AccountCircleIcon from "@mui/icons-material/AccountCircle";
import ArtistLink from "../ArtistLink/ArtistLink";
import { useFileService } from "../../../shared/hooks/FileService.hook";

type Props = {
  artists: Artist[];
  name?: boolean;
  photo?: boolean;
  about?: boolean;
  createdAt?: boolean;
  modifiedAt?: boolean;
  action?: (artist: Artist) => ReactElement;
};

export default function ArtistsList({
  artists,
  about,
  createdAt,
  name = true,
  photo = true,
  modifiedAt,
  action,
}: Props) {
  const fileService = useFileService();

  return (
    <TableContainer component={Paper}>
      <Table>
        <TableHead>
          <TableRow>
            <TableCell>#</TableCell>
            {photo && (
              <TableCell>
                <AccountCircleIcon />
              </TableCell>
            )}
            {name && <TableCell>Name</TableCell>}
            {about && <TableCell>About</TableCell>}
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
          {artists.map((artist, index) => (
            <TableRow key={index}>
              <TableCell>{index + 1}</TableCell>
              {photo && (
                <TableCell>
                  <Avatar src={fileService.getLocation(artist.photoLocation)} />
                </TableCell>
              )}
              {name && (
                <TableCell>
                  <ArtistLink artist={artist} />
                </TableCell>
              )}
              {about && <TableCell>{artist.about}</TableCell>}
              {createdAt && (
                <TableCell>
                  {format(new Date(artist.createdAt), "dd.MM.yyyy HH:mm")}
                </TableCell>
              )}
              {modifiedAt && (
                <TableCell>
                  {format(new Date(artist.modifiedAt), "dd.MM.yyyy HH:mm")}
                </TableCell>
              )}
              {action && <TableCell>{action(artist)}</TableCell>}
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </TableContainer>
  );
}
