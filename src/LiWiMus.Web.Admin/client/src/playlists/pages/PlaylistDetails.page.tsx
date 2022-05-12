import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { Playlist } from "../types/Playlist";
import Loading from "../../shared/components/Loading/Loading";
import NotFound from "../../shared/components/NotFound/NotFound";
import { Grid, Stack } from "@mui/material";
import playlistCover from "../images/playlist-cover-negative.png";
import { useSnackbar } from "notistack";
import axios from "../../shared/services/Axios";
import PlaylistImageEditor from "../components/PlaylistImageEditor/PlaylistImageEditor";
import PlaylistInfoEditor from "../components/PlaylistInfoEditor/PlaylistInfoEditor";
import PlaylistPublicityEditor from "../components/PlaylistPublicityEditor/PlaylistPublicityEditor";

const API_URL = process.env.REACT_APP_API_URL;

export default function PlaylistDetailsPage() {
  const { id } = useParams() as { id: string };
  const [playlist, setPlaylist] = useState<Playlist>();
  const [playlistPhoto, setPlaylistPhoto] = useState<string>("");
  const [loading, setLoading] = useState(true);
  const { enqueueSnackbar } = useSnackbar();

  const setPlaylistWithPhoto = (playlist: Playlist) => {
    const photoLocation = playlist.photoLocation
      ? API_URL + playlist.photoLocation
      : playlistCover;
    setPlaylist({ ...playlist });
    setPlaylistPhoto(photoLocation);
  };

  useEffect(() => {
    axios
      .get(`/playlists/${id}`)
      .then((response) => {
        const playlist = response.data as Playlist;
        setPlaylistWithPhoto(playlist);
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

  return (
    <>
      <h1>
        {playlist.name} #{playlist.id}
      </h1>

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
          <PlaylistImageEditor
            playlist={playlist}
            playlistPhoto={playlistPhoto}
            setPlaylistWithPhoto={setPlaylistWithPhoto}
          />
        </Grid>
        <Grid item xs={12} md={8} lg={4}>
          <Stack direction={"column"} spacing={2} alignItems={"end"}>
            <PlaylistInfoEditor
              id={id}
              dto={{ name: playlist.name }}
              setDto={(dto) => {
                setPlaylist({ ...playlist, ...dto });
              }}
            />
            <PlaylistPublicityEditor
              id={id}
              dto={{ isPublic: playlist.isPublic }}
              setDto={(dto) => {
                setPlaylist({ ...playlist, ...dto });
              }}
            />
          </Stack>
        </Grid>
        <Grid item xs={12} md={8} lg={4}>
          test
        </Grid>
      </Grid>
    </>
  );
}
