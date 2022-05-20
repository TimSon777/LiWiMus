import React, { useState } from "react";
import { Album } from "../../types/Album";
import { Stack, Tab, Tabs, Typography } from "@mui/material";
import TabPanel from "../../../shared/components/TabPanel/TabPanel";
import {
  DefaultPaginatedData,
  PaginatedData,
} from "../../../shared/types/PaginatedData";
import { Artist } from "../../../artists/types/Artist";
import AlbumArtistsSearchInput from "../AlbumArtistsSearchInput/AlbumArtistsSearchInput";
import ArtistsInAlbum from "../ArtistsInAlbum/ArtistsInAlbum";
import ArtistsInSearch from "../ArtistsInSearch/ArtistsInSearch";

type Props = {
  album: Album;
  setAlbum: (album: Album) => void;
};

export default function AlbumArtists({ album, setAlbum }: Props) {
  const [artistsInAlbum, setArtistsInAlbum] =
    useState<PaginatedData<Artist>>(DefaultPaginatedData);
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
          <Typography variant={"h5"} component={Tab} label={"Album artists"} />
          <Typography variant={"h5"} component={Tab} label={"Search"} />
        </Stack>
        <TabPanel value={value} index={1}>
          <AlbumArtistsSearchInput
            artistsInAlbum={artistsInAlbum.data}
            setArtists={setArtistsInSearch}
            filter={searchFilter}
            setFilter={setSearchFilter}
            setLoading={setSearchLoading}
          />
        </TabPanel>
      </Stack>

      <TabPanel value={value} index={0}>
        <ArtistsInAlbum
          album={album}
          setAlbum={setAlbum}
          artists={artistsInAlbum}
          setArtists={setArtistsInAlbum}
        />
      </TabPanel>
      <TabPanel value={value} index={1}>
        <ArtistsInSearch
          album={album}
          setAlbum={setAlbum}
          artists={artistsInSearch}
          setArtists={setArtistsInSearch}
          filter={searchFilter}
          loading={searchLoading}
          artistsInAlbum={artistsInAlbum}
          setArtistsInAlbum={setArtistsInAlbum}
        />
      </TabPanel>
    </>
  );
}
