import React from "react";
import Loading from "../../../../shared/components/Loading/Loading";
import InfiniteScroll from "react-infinite-scroll-component";
import {
  Button,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
} from "@mui/material";
import PlaylistTrackItem from "../PlaylistTrackItem/PlaylistTrackItem";
import { PaginatedData } from "../../../../shared/types/PaginatedData";
import { Track } from "../../../../tracks/types/Track";
import PlaylistService from "../../../Playlist.service";
import { Playlist } from "../../../types/Playlist";
import { useNotifier } from "../../../../shared/hooks/Notifier.hook";

type Props = {
  playlist: Playlist;
  setPlaylist: (playlist: Playlist) => void;
  loading: boolean;
  tracks: PaginatedData<Track>;
  setTracks: (tracks: PaginatedData<Track>) => void;
  fetchMoreTracks: () => Promise<void>;
};

export default function PlaylistTrackSearchTable({
  loading,
  tracks,
  setTracks,
  fetchMoreTracks,
  playlist,
  setPlaylist,
}: Props) {
  const { showError, showSuccess } = useNotifier();

  const addTrack = async (track: Track) => {
    try {
      await PlaylistService.addTrack(playlist, track);
      setPlaylist({ ...playlist, tracksCount: tracks.totalItems - 1 });
      setTracks({
        ...tracks,
        data: tracks.data.filter((t) => t.id !== track.id),
        totalItems: tracks.totalItems - 1,
      });
      showSuccess("Track added to playlist");
    } catch (e) {
      showError(e);
    }
  };

  if (loading) {
    return <Loading />;
  }

  if (tracks.data.length === 0) {
    return <></>;
  }

  return (
    <InfiniteScroll
      dataLength={tracks.data.length}
      hasMore={tracks.hasMore}
      loader={<Loading />}
      next={fetchMoreTracks}
    >
      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>#</TableCell>
              <TableCell>Name</TableCell>
              <TableCell>Artists</TableCell>
              <TableCell>Album</TableCell>
              <TableCell />
            </TableRow>
          </TableHead>
          <TableBody>
            {tracks.data.map((track, index) => (
              <PlaylistTrackItem
                key={index}
                index={index}
                track={track}
                renderAction={() => (
                  <Button
                    variant="outlined"
                    color={"secondary"}
                    sx={{ borderRadius: "20px", px: 4 }}
                    onClick={() => addTrack(track)}
                  >
                    Add
                  </Button>
                )}
              />
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    </InfiniteScroll>
  );
}
