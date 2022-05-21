import React, { useState } from "react";
import { Artist } from "../../types/Artist";
import {
  DefaultPaginatedData,
  PaginatedData,
} from "../../../shared/types/PaginatedData";
import { User } from "../../../users/types/User";
import { Stack, Tab, Tabs, Typography } from "@mui/material";
import TabPanel from "../../../shared/components/TabPanel/TabPanel";
import ArtistUsersSearchInput from "../ArtistUsersSearchInput/ArtistUsersSearchInput";
import ArtistUsersSearch from "../ArtistUsersSearch/ArtistUsersSearch";
import ArtistOwners from "../ArtistOwners/ArtistOwners";

type Props = {
  artist: Artist;
};

export default function ArtistUsers({ artist }: Props) {
  const [owners, setOwners] = useState<User[]>([]);
  const [searchUsers, setSearchUsers] =
    useState<PaginatedData<User>>(DefaultPaginatedData);
  const [searchFilter, setSearchFilter] = useState("");
  const [searchLoading, setSearchLoading] = useState(false);

  const [tab, setTab] = useState(0);

  const handleChangeTab = (event: React.SyntheticEvent, newTab: number) => {
    setTab(newTab);
  };

  return (
    <>
      <Stack direction={"row"} spacing={2}>
        <Stack
          direction={"row"}
          spacing={2}
          component={Tabs}
          value={tab}
          onChange={handleChangeTab}
        >
          <Typography variant={"h5"} component={Tab} label={"Artist owners"} />
          <Typography variant={"h5"} component={Tab} label={"Search"} />
        </Stack>
        <TabPanel value={tab} index={1}>
          <ArtistUsersSearchInput
            exclude={owners}
            setSearch={setSearchUsers}
            filter={searchFilter}
            setFilter={setSearchFilter}
            setLoading={setSearchLoading}
          />
        </TabPanel>
      </Stack>

      <TabPanel value={tab} index={0}>
        <ArtistOwners artist={artist} owners={owners} setOwners={setOwners} />
      </TabPanel>
      <TabPanel value={tab} index={1}>
        <ArtistUsersSearch
          artist={artist}
          users={searchUsers}
          setUsers={setSearchUsers}
          filter={searchFilter}
          loading={searchLoading}
          owners={owners}
          setOwners={setOwners}
        />
      </TabPanel>
    </>
  );
}
