import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import PlaylistsService from "../services/Playlists.service";
import { Playlist } from "../types/Playlist";
import Loading from "../components/Loading/Loading";
import NotFound from "../components/NotFound/NotFound";
import { Grid } from "@mui/material";
import ImageEditor from "../components/ImageEditor/ImageEditor";
import playlistCover from "../images/playlist-cover-negative.png";

export default function PlaylistDetailsPage() {
  const { id } = useParams() as { id: string };
  const [playlist, setPlaylist] = useState<Playlist | null>();
  const [loading, setLoading] = useState(true);

  async function fetchData(promise: Promise<Playlist | null>) {
    const res = await promise;
    if (res) {
      setPlaylist(res);
    }
  }

  useEffect(() => {
    fetchData(PlaylistsService.getById(id)).then(() => setLoading(false));
  }, [id]);

  if (loading) {
    return <Loading />;
  }

  if (!playlist) {
    return <NotFound />;
  }

  const updatePlaylistPhoto = async (formData: FormData) => {
    formData.append("id", id);
    await fetchData(PlaylistsService.update(formData));
    return playlist;
  };

  const removePlaylistPhoto = async () => {
    await fetchData(PlaylistsService.removePhoto(id));
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
            src={playlist.photoPath}
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
