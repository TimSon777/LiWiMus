import React from "react";
import { Playlist } from "../../../types/Playlist";
import { PaginatedData } from "../../../../shared/types/PaginatedData";
import { Track } from "../../../../tracks/types/Track";
import { useNotifier } from "../../../../shared/hooks/Notifier.hook";
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
import TrackService from "../../../../tracks/Track.service";
import PlaylistService from "../../../Playlist.service";

type Props = {
  playlist: Playlist;
  setPlaylist: (playlist: Playlist) => void;
  tracks: PaginatedData<Track>;
  setTracks: (tracks: PaginatedData<Track>) => void;
  tracksInPlaylist: PaginatedData<Track>;
  setTracksInPlaylist: (tracks: PaginatedData<Track>) => void;
  filter: string;
  loading: boolean;
};

export default function TracksInSearch({
  playlist,
  setPlaylist,
  tracks,
  setTracks,
  filter,
  loading,
  tracksInPlaylist,
  setTracksInPlaylist,
}: Props) {
  const { showError, showSuccess } = useNotifier();

  const fetchMore = async () => {
    try {
      const newTracks = await TrackService.getTracks({
        filters: [
          { columnName: "name", operator: "cnt", value: filter },
          {
            columnName: "id",
            operator: "-in",
            value: tracksInPlaylist.data.map((track) => track.id),
          },
        ],
        page: { pageNumber: tracks.actualPage + 1, numberOfElementsOnPage: 10 },
      });
      setTracks({ ...newTracks, data: [...tracks.data, ...newTracks.data] });
    } catch (e) {
      showError(e);
    }
  };

  const addTrack = async (track: Track) => {
    try {
      await PlaylistService.addTrack(playlist, track);
      setPlaylist({ ...playlist, tracksCount: playlist.tracksCount + 1 });
      setTracksInPlaylist({
        ...tracksInPlaylist,
        totalItems: tracksInPlaylist.totalItems + 1,
        data: [...tracksInPlaylist.data, track],
      });
      setTracks({
        ...tracks,
        totalItems: tracks.totalItems - 1,
        data: tracks.data.filter((t) => t.id !== track.id),
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
      next={fetchMore}
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
