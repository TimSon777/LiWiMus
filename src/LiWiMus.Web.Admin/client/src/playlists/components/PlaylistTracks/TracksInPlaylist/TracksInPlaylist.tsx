import React, { useEffect, useState } from "react";
import { Track } from "../../../../tracks/types/Track";
import { Playlist } from "../../../types/Playlist";
import PlaylistService from "../../../Playlist.service";
import Loading from "../../../../shared/components/Loading/Loading";
import {
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
} from "@mui/material";
import AlertDialog from "../../../../shared/components/AlertDialog/AlertDialog";
import PlaylistTrackItem from "../PlaylistTrackItem/PlaylistTrackItem";
import { useNotifier } from "../../../../shared/hooks/Notifier.hook";
import InfiniteScroll from "react-infinite-scroll-component";
import { PaginatedData } from "../../../../shared/types/PaginatedData";

type Props = {
  playlist: Playlist;
  setPlaylist: (playlist: Playlist) => void;
  tracks: PaginatedData<Track>;
  setTracks: (tracks: PaginatedData<Track>) => void;
};

export default function TracksInPlaylist({
  playlist,
  setPlaylist,
  tracks,
  setTracks,
}: Props) {
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const { showError, showSuccess } = useNotifier();

  const fetchMore = async () => {
    try {
      const newTracks = await PlaylistService.getTracks(
        playlist,
        tracks.actualPage + 1,
        10
      );
      setTracks({
        ...newTracks,
        data: [...tracks.data, ...newTracks.data],
        hasMore: newTracks.actualPage < newTracks.totalPages,
      });
      setPlaylist({ ...playlist, tracksCount: newTracks.totalItems });
    } catch (e) {
      showError(e);
    }
  };

  useEffect(() => {
    if (tracks.hasMore) {
      setIsLoading(true);
      fetchMore().then(() => {
        setIsLoading(false);
      });
    } else {
      setIsLoading(false);
    }
  }, []);

  const removeTrack = async (track: Track) => {
    try {
      await PlaylistService.removeTrack(playlist, track);
      setPlaylist({
        ...playlist,
        tracksCount: tracks.totalItems - 1,
      });
      setTracks({
        ...tracks,
        data: tracks.data.filter((t) => t.id !== track.id),
        totalItems: tracks.totalItems - 1,
      });
      showSuccess("Track removed from playlist");
    } catch (e) {
      showError(e);
    }
  };

  if (isLoading) {
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
                  <AlertDialog
                    onAgree={() => removeTrack(track)}
                    title={"Remove track from playlist?"}
                    agreeText={"Remove"}
                    disagreeText={"Cancel"}
                    buttonText={"Remove"}
                  />
                )}
              />
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    </InfiniteScroll>
  );
}
