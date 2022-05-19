import React, { useState } from "react";
import {
  FormControlLabel,
  Paper,
  Switch,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
} from "@mui/material";
import TrackService from "../../../tracks/Track.service";
import {
  DefaultPaginatedData,
  PaginatedData,
} from "../../../shared/types/PaginatedData";
import { Track } from "../../../tracks/types/Track";
import { Album } from "../../types/Album";
import { useNotifier } from "../../../shared/hooks/Notifier.hook";
import InfiniteScroll from "react-infinite-scroll-component";
import Loading from "../../../shared/components/Loading/Loading";
import TrackLink from "../../../tracks/components/TrackLink/TrackLink";
import ArtistsList from "../../../artists/components/ArtistsList/ArtistsList";
import AlbumLink from "../AlbumLink/AlbumLink";
import NotFound from "../../../shared/components/NotFound/NotFound";

type Props = {
  album: Album;
};

export default function AlbumTracks({ album }: Props) {
  const [open, setOpen] = useState(false);
  const [tracks, setTracks] =
    useState<PaginatedData<Track>>(DefaultPaginatedData);
  const { showError } = useNotifier();

  const showHandler = () => {
    setOpen(!open);
  };

  const fetchMore = async () => {
    try {
      const newTracks = await TrackService.getTracks({
        page: {
          pageNumber: tracks.actualPage + 1,
          numberOfElementsOnPage: 10,
        },
        filters: [{ columnName: "album", operator: "in", value: [album.id] }],
      });
      setTracks({
        ...newTracks,
        data: [...tracks.data, ...newTracks.data],
        hasMore: newTracks.actualPage < newTracks.totalPages,
      });
    } catch (e) {
      showError(e);
    }
  };

  return (
    <>
      <FormControlLabel
        control={<Switch checked={open} onChange={showHandler} />}
        label="Show tracks"
        componentsProps={{ typography: { fontSize: "2rem" } }}
      />
      {open && (
        <InfiniteScroll
          dataLength={tracks.data.length}
          hasMore={tracks.hasMore}
          loader={<Loading />}
          next={fetchMore}
        >
          {tracks.data.length > 0 ? (
            <TableContainer component={Paper}>
              <Table>
                <TableHead>
                  <TableRow>
                    <TableCell>#</TableCell>
                    <TableCell>Name</TableCell>
                    <TableCell>Artists</TableCell>
                    <TableCell>Album</TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {tracks.data.map((track, index) => (
                    <TableRow>
                      <TableCell>{index + 1}</TableCell>
                      <TableCell>
                        <TrackLink track={track} cover />
                      </TableCell>
                      <TableCell>
                        <ArtistsList artists={track.artists} />
                      </TableCell>
                      <TableCell>
                        <AlbumLink album={track.album} />
                      </TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </TableContainer>
          ) : (
            <NotFound />
          )}
        </InfiniteScroll>
      )}
    </>
  );
}
