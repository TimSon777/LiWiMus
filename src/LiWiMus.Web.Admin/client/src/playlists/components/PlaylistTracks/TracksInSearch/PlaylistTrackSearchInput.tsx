import React, { useState } from "react";
import { Box, IconButton, TextField } from "@mui/material";
import SearchIcon from "@mui/icons-material/Search";
import { PaginatedData } from "../../../../shared/types/PaginatedData";
import { Track } from "../../../../tracks/types/Track";
import { useNotifier } from "../../../../shared/hooks/Notifier.hook";
import { useTrackService } from "../../../../tracks/TrackService.hook";

type Props = {
  tracksInPlaylist: Track[];
  filter: string;
  setFilter: (filter: string) => void;
  setTracks: (tracks: PaginatedData<Track>) => void;
  setLoading: (loading: boolean) => void;
};

export default function PlaylistTrackSearchInput({
  tracksInPlaylist,
  filter,
  setFilter,
  setTracks,
  setLoading,
}: Props) {
  const trackService = useTrackService();

  const [value, setValue] = useState(filter);
  const { showError } = useNotifier();

  const handler = async () => {
    if (!value) {
      return;
    }

    setLoading(true);
    try {
      const tracks = await trackService.getTracks({
        filters: [
          { columnName: "name", operator: "cnt", value },
          {
            columnName: "id",
            operator: "-in",
            value: [0, ...tracksInPlaylist.map((track) => track.id)],
          },
        ],
        page: { pageNumber: 1, numberOfElementsOnPage: 10 },
      });
      setTracks(tracks);
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
