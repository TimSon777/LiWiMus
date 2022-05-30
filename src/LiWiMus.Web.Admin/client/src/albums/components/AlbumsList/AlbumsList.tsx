import React, { ReactElement } from "react";
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
import { Album } from "../../types/Album";
import AlbumLink from "../AlbumLink/AlbumLink";
import ArtistsLinks from "../../../artists/components/ArtistsLinks/ArtistsLinks";
import { useFileService } from "../../../shared/hooks/FileService.hook";

type Props = {
  albums: Album[];
  title?: boolean;
  cover?: boolean;
  publishedAt?: boolean;
  createdAt?: boolean;
  modifiedAt?: boolean;
  artists?: boolean;
  action?: (album: Album) => ReactElement;
};

export default function AlbumsList({
  artists,
  action,
  cover,
  createdAt,
  publishedAt,
  albums,
  title,
  modifiedAt,
}: Props) {
  const fileService = useFileService();

  return (
    <TableContainer component={Paper}>
      <Table>
        <TableHead>
          <TableRow>
            <TableCell>#</TableCell>
            {cover && (
              <TableCell>
                <AccountCircleIcon />
              </TableCell>
            )}
            {title && <TableCell>Title</TableCell>}
            {artists && <TableCell>Artists</TableCell>}
            {publishedAt && <TableCell>Published at</TableCell>}
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
          {albums.map((album, index) => (
            <TableRow key={index}>
              <TableCell>{index + 1}</TableCell>
              {cover && (
                <TableCell>
                  <Avatar src={fileService.getLocation(album.coverLocation)} />
                </TableCell>
              )}
              {title && (
                <TableCell>
                  <AlbumLink album={album} />
                </TableCell>
              )}
              {artists && (
                <TableCell>
                  {<ArtistsLinks artists={album.artists} />}
                </TableCell>
              )}
              {publishedAt && <TableCell>{album.publishedAt}</TableCell>}
              {createdAt && (
                <TableCell>
                  {format(new Date(album.createdAt), "dd.MM.yyyy HH:mm")}
                </TableCell>
              )}
              {modifiedAt && (
                <TableCell>
                  {format(new Date(album.modifiedAt), "dd.MM.yyyy HH:mm")}
                </TableCell>
              )}
              {action && <TableCell>{action(album)}</TableCell>}
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </TableContainer>
  );
}
