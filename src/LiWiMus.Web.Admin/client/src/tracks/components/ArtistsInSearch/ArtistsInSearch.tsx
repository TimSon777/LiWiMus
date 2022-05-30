import React, { useEffect } from "react";
import { Track } from "../../types/Track";
import { PaginatedData } from "../../../shared/types/PaginatedData";
import { Artist } from "../../../artists/types/Artist";
import { useNotifier } from "../../../shared/hooks/Notifier.hook";
import Loading from "../../../shared/components/Loading/Loading";
import InfiniteScroll from "react-infinite-scroll-component";
import ArtistsList from "../../../artists/components/ArtistsList/ArtistsList";
import { Button } from "@mui/material";
import { useTrackService } from "../../TrackService.hook";
import { useArtistService } from "../../../artists/ArtistService.hook";

type Props = {
  track: Track;
  setTrack: (track: Track) => void;
  artists: PaginatedData<Artist>;
  setArtists: (artists: PaginatedData<Artist>) => void;
  filter: string;
  loading: boolean;
};

export default function ArtistsInSearch({
  artists,
  setArtists,
  track,
  setTrack,
  filter,
  loading,
}: Props) {
  const trackService = useTrackService();
  const artistService = useArtistService();

  const { showError, showSuccess } = useNotifier();

  const getArtists = async (filter: string, page: number) => {
    return await artistService.getArtists({
      filters: [
        { columnName: "name", operator: "cnt", value: filter },
        {
          columnName: "id",
          operator: "-in",
          value: [0, ...track.artists.map((artist) => artist.id)],
        },
      ],
      page: {
        pageNumber: page,
        numberOfElementsOnPage: 10,
      },
    });
  };

  const fetchMore = async () => {
    try {
      const newArtists = await getArtists(filter, artists.actualPage + 1);
      setArtists({
        ...newArtists,
        data: [...artists.data, ...newArtists.data],
      });
    } catch (e) {
      showError(e);
    }
  };

  useEffect(() => {
    getArtists("", 1).then(setArtists);
  }, []);

  const addArtist = async (artist: Artist) => {
    try {
      await trackService.addArtist(track, artist);
      setArtists({
        ...artists,
        totalItems: artists.totalItems - 1,
        data: artists.data.filter((t) => t.id !== artist.id),
      });
      setTrack({ ...track, artists: [...track.artists, artist] });
      showSuccess("Artist added to track");
    } catch (e) {
      showError(e);
    }
  };

  if (loading) {
    return <Loading />;
  }

  if (artists.data.length === 0) {
    return <></>;
  }

  return (
    <InfiniteScroll
      dataLength={artists.data.length}
      hasMore={artists.hasMore}
      loader={<Loading />}
      next={fetchMore}
    >
      <ArtistsList
        artists={artists.data}
        photo
        name
        action={(artist) => (
          <Button
            variant="outlined"
            color={"secondary"}
            sx={{ borderRadius: "20px", px: 4 }}
            onClick={() => addArtist(artist)}
          >
            Add
          </Button>
        )}
      />
    </InfiniteScroll>
  );
}
