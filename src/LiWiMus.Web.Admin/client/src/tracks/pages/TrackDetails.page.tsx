import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { Track } from "../types/Track";
import { useNotifier } from "../../shared/hooks/Notifier.hook";
import Loading from "../../shared/components/Loading/Loading";
import NotFound from "../../shared/components/NotFound/NotFound";
import { Grid, Paper, Stack, Typography } from "@mui/material";
import TrackInfoEditor from "../components/TrackInfoEditor/TrackInfoEditor";
import TrackDeleter from "../components/TrackDeleter/TrackDeleter";
import TrackFileEditor from "../components/TrackFileEditor/TrackFileEditor";
import HoverImage from "../../shared/components/HoverImage/HoverImage";
import AlbumLink from "../../albums/components/AlbumLink/AlbumLink";
import GenreCard from "../components/GenreCard/GenreCard";
import AddGenreCard from "../components/AddGenreCard/AddGenreCard";
import TrackArtists from "../components/TrackArtists/TrackArtists";
import { useTrackService } from "../TrackService.hook";
import { useFileService } from "../../shared/hooks/FileService.hook";

export default function TrackDetailsPage() {
  const trackService = useTrackService();
  const fileService = useFileService();

  const { id } = useParams() as { id: string };
  const [track, setTrack] = useState<Track>();
  const [loading, setLoading] = useState(true);
  const { showError } = useNotifier();

  useEffect(() => {
    trackService
      .get(id)
      .then((track) => setTrack(track))
      .catch(showError)
      .then(() => setLoading(false));
  }, [id]);

  if (loading) {
    return <Loading />;
  }

  if (!track) {
    return <NotFound />;
  }

  return (
    <Grid container spacing={5} justifyContent={"center"} sx={{ py: 5 }}>
      <Grid item xs={12} md={10} lg={8}>
        <Paper sx={{ p: 4 }} elevation={10}>
          <Typography variant={"h3"} component={"div"}>
            {track.name}
          </Typography>

          <Grid container spacing={2}>
            <Grid item xs={12} md={6}>
              <Stack spacing={2} alignItems={"center"}>
                <HoverImage
                  src={fileService.getLocation(track.album.coverLocation)}
                  alt={track.album.title}
                  size={250}
                >
                  <AlbumLink album={track.album} />
                </HoverImage>
                <TrackFileEditor track={track} setTrack={setTrack} />
                <TrackDeleter track={track} setTrack={setTrack} />
              </Stack>
            </Grid>

            <Grid item xs={12} md={6}>
              <TrackInfoEditor track={track} setTrack={setTrack} />
            </Grid>
          </Grid>
        </Paper>
      </Grid>

      <Grid item xs={12} md={10} lg={8}>
        <Typography variant={"h4"} component={"div"}>
          Genres
        </Typography>
        <Grid container spacing={3}>
          {track.genres.map((genre, index) => (
            <Grid item xs={4} md={3} lg={2} key={index}>
              <GenreCard genre={genre} track={track} setTrack={setTrack} />
            </Grid>
          ))}
          <Grid item xs={4} md={3} lg={2}>
            <AddGenreCard track={track} setTrack={setTrack} />
          </Grid>
        </Grid>
      </Grid>

      <Grid item xs={12} md={10} lg={8}>
        <TrackArtists track={track} setTrack={setTrack} />
      </Grid>
    </Grid>
  );
}
