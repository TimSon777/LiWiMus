import React, { useState } from "react";
import { User } from "../../../users/types/User";
import { PaginatedData } from "../../../shared/types/PaginatedData";
import { useNotifier } from "../../../shared/hooks/Notifier.hook";
import { Box, IconButton, TextField } from "@mui/material";
import SearchIcon from "@mui/icons-material/Search";
import UserService from "../../../users/User.service";

type Props = {
  exclude: User[];
  setSearch: (search: PaginatedData<User>) => void;
  filter: string;
  setFilter: (filter: string) => void;
  setLoading: (loading: boolean) => void;
};

export default function ArtistUsersSearchInput({
  setSearch,
  setFilter,
  filter,
  exclude,
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
      const users = await UserService.getUsers({
        filters: [
          { columnName: "userName", operator: "cnt", value },
          {
            columnName: "id",
            operator: "-in",
            value: [0, ...exclude.map((user) => user.id)],
          },
        ],
        page: { pageNumber: 1, numberOfElementsOnPage: 10 },
      });
      setSearch(users);
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
