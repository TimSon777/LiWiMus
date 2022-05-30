import React, { useState } from "react";
import { FormControlLabel, Switch } from "@mui/material";
import {
  DefaultPaginatedData,
  PaginatedData,
} from "../../../shared/types/PaginatedData";
import { Track } from "../../../tracks/types/Track";
import { Album } from "../../types/Album";
import { useNotifier } from "../../../shared/hooks/Notifier.hook";
import InfiniteScroll from "react-infinite-scroll-component";
import Loading from "../../../shared/components/Loading/Loading";
import TracksList from "../../../tracks/components/TracksList/TracksList";
import { useTrackService } from "../../../tracks/TrackService.hook";

type Props = {
  album: Album;
};

export default function AlbumTracks({ album }: Props) {
  const trackService = useTrackService();

  const [open, setOpen] = useState(false);
  const [tracks, setTracks] =
    useState<PaginatedData<Track>>(DefaultPaginatedData);
  const { showError } = useNotifier();

  const showHandler = () => {
    setOpen(!open);
  };

  const fetchMore = async () => {
    try {
      const newTracks = await trackService.getTracks({
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
          <TracksList
            tracks={tracks.data}
            name
            artists
            publishedAt
            duration
            album={false}
            albumCover={false}
          />
        </InfiniteScroll>
      )}
    </>
  );
}
