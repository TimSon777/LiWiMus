import React, { useState } from "react";
import { Track } from "../../types/Track";
import {
  DefaultPaginatedData,
  PaginatedData,
} from "../../../shared/types/PaginatedData";
import { Artist } from "../../../artists/types/Artist";
import { Stack, Tab, Tabs, Typography } from "@mui/material";
import TabPanel from "../../../shared/components/TabPanel/TabPanel";
import TrackArtistsSearchInput from "../TrackArtistsSearchInput/TrackArtistsSearchInput";
import ArtistsInTrack from "../ArtistsInTrack/ArtistsInTrack";
import ArtistsInSearch from "../ArtistsInSearch/ArtistsInSearch";

type Props = {
  track: Track;
  setTrack: (track: Track) => void;
};

export default function TrackArtists({ track, setTrack }: Props) {
  const [artistsInSearch, setArtistsInSearch] =
    useState<PaginatedData<Artist>>(DefaultPaginatedData);
  const [searchFilter, setSearchFilter] = useState("");
  const [searchLoading, setSearchLoading] = useState(false);

  const [value, setValue] = React.useState(0);

  const handleChange = (event: React.SyntheticEvent, newValue: number) => {
    setValue(newValue);
  };

  return (
    <>
      <Stack direction={"row"} spacing={2}>
        <Stack
          direction={"row"}
          spacing={2}
          component={Tabs}
          value={value}
          onChange={handleChange}
        >
          <Typography variant={"h5"} component={Tab} label={"Track artists"} />
          <Typography variant={"h5"} component={Tab} label={"Search"} />
        </Stack>
        <TabPanel value={value} index={1}>
          <TrackArtistsSearchInput
            artistsInTrack={track.artists}
            setArtists={setArtistsInSearch}
            filter={searchFilter}
            setFilter={setSearchFilter}
            setLoading={setSearchLoading}
          />
        </TabPanel>
      </Stack>

      <TabPanel value={value} index={0}>
        <ArtistsInTrack track={track} setTrack={setTrack} />
      </TabPanel>
      <TabPanel value={value} index={1}>
        <ArtistsInSearch
          track={track}
          setTrack={setTrack}
          artists={artistsInSearch}
          setArtists={setArtistsInSearch}
          filter={searchFilter}
          loading={searchLoading}
        />
      </TabPanel>
    </>
  );
}
