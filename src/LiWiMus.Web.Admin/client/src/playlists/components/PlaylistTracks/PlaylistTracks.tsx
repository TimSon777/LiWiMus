import React, { useState } from "react";
import { Grid, Stack, Tab, Tabs, Typography } from "@mui/material";
import { Playlist } from "../../types/Playlist";
import TracksInPlaylist from "./TracksInPlaylist/TracksInPlaylist";
import TracksInSearch from "./TracksInSearch/TracksInSearch";
import {
  DefaultPaginatedData,
  PaginatedData,
} from "../../../shared/types/PaginatedData";
import { Track } from "../../../tracks/types/Track";
import TabPanel from "../../../shared/components/TabPanel/TabPanel";
import PlaylistTrackSearchInput from "./TracksInSearch/PlaylistTrackSearchInput";

type Props = {
  playlist: Playlist;
  setPlaylist: (playlist: Playlist) => void;
};

export default function PlaylistTracks({ playlist, setPlaylist }: Props) {
  const [tracksInPlaylist, setTracksInPlaylist] =
    useState<PaginatedData<Track>>(DefaultPaginatedData);
  const [tracksInSearch, setTracksInSearch] =
    useState<PaginatedData<Track>>(DefaultPaginatedData);
  const [searchFilter, setSearchFilter] = useState("");
  const [searchLoading, setSearchLoading] = useState(false);

  const [value, setValue] = React.useState(0);

  const handleChange = (event: React.SyntheticEvent, newValue: number) => {
    setValue(newValue);
  };

  return (
    <Grid item xs={12}>
      <Stack direction={"row"} spacing={2}>
        <Stack
          direction={"row"}
          spacing={2}
          component={Tabs}
          value={value}
          onChange={handleChange}
        >
          <Typography
            variant={"h5"}
            component={Tab}
            label={"Tracks in playlist"}
          />
          <Typography variant={"h5"} component={Tab} label={"Track search"} />
        </Stack>
        <TabPanel value={value} index={1}>
          <PlaylistTrackSearchInput
            tracksInPlaylist={tracksInPlaylist.data}
            filter={searchFilter}
            setFilter={setSearchFilter}
            setTracks={setTracksInSearch}
            setLoading={setSearchLoading}
          />
        </TabPanel>
      </Stack>

      <TabPanel value={value} index={0}>
        <TracksInPlaylist
          playlist={playlist}
          setPlaylist={setPlaylist}
          tracks={tracksInPlaylist}
          setTracks={setTracksInPlaylist}
        />
      </TabPanel>
      <TabPanel value={value} index={1}>
        <TracksInSearch
          playlist={playlist}
          setPlaylist={setPlaylist}
          tracks={tracksInSearch}
          setTracks={setTracksInSearch}
          filter={searchFilter}
          loading={searchLoading}
          tracksInPlaylist={tracksInPlaylist}
          setTracksInPlaylist={setTracksInPlaylist}
        />
      </TabPanel>
    </Grid>
  );
}
