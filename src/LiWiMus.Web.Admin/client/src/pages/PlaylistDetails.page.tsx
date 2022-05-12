import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { Playlist } from "../types/Playlist";
import Loading from "../components/Loading/Loading";
import NotFound from "../components/NotFound/NotFound";
import { Grid } from "@mui/material";
import ImageEditor from "../components/ImageEditor/ImageEditor";
import playlistCover from "../images/playlist-cover-negative.png";
import { useSnackbar } from "notistack";
import axios from "../services/Axios";

export default function PlaylistDetailsPage() {
  const { id } = useParams() as { id: string };
  const [playlist, setPlaylist] = useState<Playlist | null>();
  const [loading, setLoading] = useState(true);
  const { enqueueSnackbar } = useSnackbar();

  useEffect(() => {
    axios
      .get(`/playlists/${id}`)
      .then((response) => {
        setPlaylist(response.data as Playlist);
      })
      .catch((error) => enqueueSnackbar(error.message, { variant: "error" }))
      .then(() => setLoading(false));
  }, [id]);

  if (loading) {
    return <Loading />;
  }

  if (!playlist) {
    return <NotFound />;
  }

  const updatePlaylistPhoto = async (photo: File) => {
    try {
      if (playlist.photoLocation) {
        await axios.delete(playlist.photoLocation);
      }

      const formData = new FormData();
      formData.append("file", photo);
      const { data } = await axios.post("/files", formData, {
        headers: { "Content-Type": "multipart/form-data" },
      });
      const photoLocation = data.location as string;
      const updateDto = { id, photoLocation };
      const response = await axios.patch("/playlists", updateDto);
      setPlaylist(response.data as Playlist);
      enqueueSnackbar("Photo updated", { variant: "success" });
    } catch (error) {
      // @ts-ignore
      enqueueSnackbar(error.message, { variant: "error" });
    }
    return playlist;
  };

  const removePlaylistPhoto = async () => {
    if (!playlist.photoLocation) {
      return playlist;
    }
    try {
      await axios.delete(playlist.photoLocation);
      const response = await axios.post(`/playlists/${id}/removePhoto`);
      setPlaylist(response.data as Playlist);
      enqueueSnackbar("Photo removed", { variant: "success" });
    } catch (error) {
      // @ts-ignore
      enqueueSnackbar(error.message, { variant: "error" });
    }
    return playlist;
  };

  return (
    <>
      <Grid container spacing={2} justifyContent={"center"}>
        <Grid
          item
          xs={12}
          md={8}
          lg={4}
          sx={{
            alignItems: "center",
            display: "flex",
            flexDirection: "column",
          }}
        >
          <ImageEditor
            width={250}
            src={playlist.photoLocation}
            imagePlaceholderSrc={playlistCover}
            updatePhoto={updatePlaylistPhoto}
            removePhoto={removePlaylistPhoto}
          />
          <h1>{playlist.name}</h1>
        </Grid>
        <Grid item xs={12} md={8} lg={4}>
          test
        </Grid>
        <Grid item xs={12} md={8} lg={4}>
          test
        </Grid>
      </Grid>
    </>
  );
}
