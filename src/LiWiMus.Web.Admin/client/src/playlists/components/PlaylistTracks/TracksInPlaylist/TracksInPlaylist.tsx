import React, { useEffect, useState } from "react";
import { Track } from "../../../../tracks/types/Track";
import { Playlist } from "../../../types/Playlist";
import Loading from "../../../../shared/components/Loading/Loading";
import AlertDialog from "../../../../shared/components/AlertDialog/AlertDialog";
import { useNotifier } from "../../../../shared/hooks/Notifier.hook";
import InfiniteScroll from "react-infinite-scroll-component";
import { PaginatedData } from "../../../../shared/types/PaginatedData";
import TracksList from "../../../../tracks/components/TracksList/TracksList";
import { usePlaylistService } from "../../../PlaylistService.hook";

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
  const playlistService = usePlaylistService();

  const [isLoading, setIsLoading] = useState<boolean>(true);
  const { showError, showSuccess } = useNotifier();

  const fetchMore = async () => {
    try {
      const newTracks = await playlistService.getTracks(
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
      await playlistService.removeTrack(playlist, track);
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
      <TracksList
        tracks={tracks.data}
        albumCover
        name
        artists
        album
        action={(track) => (
          <AlertDialog
            onAgree={() => removeTrack(track)}
            title={"Remove track from playlist?"}
            text={"You can add it back later"}
            agreeText={"Remove"}
            disagreeText={"Cancel"}
            buttonText={"Remove"}
          />
        )}
      />
    </InfiniteScroll>
  );
}
