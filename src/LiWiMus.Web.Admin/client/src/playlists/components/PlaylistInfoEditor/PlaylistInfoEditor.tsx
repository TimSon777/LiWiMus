import React from "react";
import { Playlist } from "../../types/Playlist";
import { Box, Button, Stack } from "@mui/material";
import ContrastTextField from "../../../shared/components/ContrastTextField/ContrastTextField";
import { SubmitHandler, useForm } from "react-hook-form";
import axios from "../../../shared/services/Axios";
import { useSnackbar } from "notistack";

type Inputs = {
  name: string;
};

type Props = {
  playlist: Playlist;
  setPlaylist: (playlist: Playlist) => void;
};

export default function PlaylistInfoEditor({ playlist, setPlaylist }: Props) {
  const { enqueueSnackbar } = useSnackbar();

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<Inputs>({ defaultValues: { name: playlist.name } });

  const saveHandler: SubmitHandler<Inputs> = async ({ name }) => {
    if (name === playlist.name) {
      return;
    }
    const dto = { id: playlist.id, name };
    try {
      const response = await axios.patch("/playlists", dto);
      enqueueSnackbar("Info updated", { variant: "success" });
      let data = response.data as Playlist;
      console.log("было", playlist.isPublic);
      console.log("стало", data.isPublic);
      setPlaylist(data);
    } catch (error) {
      // @ts-ignore
      enqueueSnackbar(error.message, { variant: "error" });
    }
  };

  const publicityHandler = async () => {
    const dto = { id: playlist.id, isPublic: !playlist.isPublic };
    try {
      const response = await axios.patch("/playlists", dto);
      let data = response.data as Playlist;
      setPlaylist(data);
      enqueueSnackbar(`Playlist is ${data.isPublic ? "public" : "private"}`, {
        variant: "success",
      });
    } catch (error) {
      // @ts-ignore
      enqueueSnackbar(error.message, { variant: "error" });
    }
  };

  return (
    <Box sx={{ display: "flex", flexDirection: "column" }}>
      <Stack direction={"column"} spacing={2}>
        <ContrastTextField
          error={!!errors.name && !!errors.name.message}
          helperText={errors.name?.message}
          label="Name"
          variant="outlined"
          fullWidth
          {...register("name", {
            required: { value: true, message: "Enter playlist name" },
          })}
        />
        <Button
          onClick={handleSubmit(saveHandler)}
          variant="contained"
          sx={{ borderRadius: "20px", px: 4, alignSelf: "flex-end" }}
        >
          Save
        </Button>
        <Button
          variant="contained"
          color={"error"}
          sx={{ borderRadius: "20px", px: 4, alignSelf: "flex-end" }}
          onClick={publicityHandler}
        >
          Make {playlist.isPublic ? "private" : "public"}
        </Button>
      </Stack>
    </Box>
  );
}
