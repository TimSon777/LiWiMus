import React, { useState } from "react";
import { PaginatedData } from "../../../shared/types/PaginatedData";
import { Artist } from "../../../artists/types/Artist";
import { useNotifier } from "../../../shared/hooks/Notifier.hook";
import ArtistService from "../../../artists/Artist.service";
import { Box, IconButton, TextField } from "@mui/material";
import SearchIcon from "@mui/icons-material/Search";

type Props = {
  artistsInTrack: Artist[];
  filter: string;
  setFilter: (filter: string) => void;
  setArtists: (artists: PaginatedData<Artist>) => void;
  setLoading: (loading: boolean) => void;
};

export default function TrackArtistsSearchInput({
  artistsInTrack,
  filter,
  setFilter,
  setArtists,
  setLoading,
}: Props) {
  const [value, setValue] = useState(filter);
  const { showError } = useNotifier();

  const handler = async () => {
    if (!value) {
      return;
    }

    setLoading(true);
    try {
      const artists = await ArtistService.getArtists({
        filters: [
          { columnName: "name", operator: "cnt", value },
          {
            columnName: "id",
            operator: "-in",
            value: [0, ...artistsInTrack.map((artist) => artist.id)],
          },
        ],
        page: { pageNumber: 1, numberOfElementsOnPage: 10 },
      });
      setArtists(artists);
      setFilter(value);
    } catch (e) {
      showError(e);
    }
    setLoading(false);
  };

  return (
    <Box sx={{ display: "flex", alignItems: "center" }}>
      <TextField
        label="Filter"
        variant="filled"
        color={"secondary"}
        sx={{
          "& .MuiFilledInput-root::before": { borderBottom: 0 },
          "& .MuiFilledInput-root:hover::before": { borderBottom: 0 },
          "& .MuiFilledInput-root::after": { borderBottom: 0 },
          "& .MuiFilledInput-root:hover::after": { borderBottom: 0 },
        }}
        inputProps={{
          sx: { backgroundColor: "background.default" },
        }}
        value={value}
        onChange={(e) => setValue(e.target.value)}
      />
      <IconButton sx={{ width: "50px", height: "50px" }} onClick={handler}>
        <SearchIcon />
      </IconButton>
    </Box>
  );
}
