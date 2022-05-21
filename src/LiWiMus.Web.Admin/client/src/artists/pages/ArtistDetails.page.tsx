import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { useNotifier } from "../../shared/hooks/Notifier.hook";
import Loading from "../../shared/components/Loading/Loading";
import NotFound from "../../shared/components/NotFound/NotFound";
import { Grid, Stack, Typography } from "@mui/material";
import ReadonlyInfo from "../../shared/components/InfoItem/ReadonlyInfo";
import { format } from "date-fns";
import InfoCard from "../../shared/components/InfoCard/InfoCard";
import { Artist } from "../types/Artist";
import ArtistService from "../Artist.service";
import ArtistImageEditor from "../components/ArtistImageEditor/ArtistImageEditor";
import ArtistInfoEditor from "../components/ArtistInfoEditor/ArtistInfoEditor";
import ArtistDeleter from "../components/ArtistDeleter/ArtistDeleter";
import ArtistUsers from "../components/ArtistUsers/ArtistUsers";
import AlbumsList from "../../albums/components/AlbumsList/AlbumsList";

export default function ArtistDetailsPage() {
  const { id } = useParams() as { id: string };
  const [artist, setArtist] = useState<Artist>();
  const [loading, setLoading] = useState(true);
  const { showError } = useNotifier();

  useEffect(() => {
    ArtistService.get(id)
      .then((artist) => {
        setArtist(artist);
      })
      .catch(showError)
      .then(() => setLoading(false));
  }, [id]);

  if (loading) {
    return <Loading />;
  }

  if (!artist) {
    return <NotFound />;
  }

  return (
    <>
      <Typography variant={"h3"} component={"div"}>
        {artist.name}
      </Typography>

      <Grid
        container
        spacing={10}
        justifyContent={"space-around"}
        sx={{ mb: 10 }}
      >
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
          <ArtistImageEditor artist={artist} setArtist={setArtist} />
        </Grid>

        <Grid item xs={12} md={8} lg={4}>
          <Stack direction={"column"} spacing={3} alignItems={"end"}>
            <ReadonlyInfo name={"ID"} value={artist.id} />
            <ReadonlyInfo
              name={"Created at"}
              value={format(new Date(artist.createdAt), "dd.mm.yyyy, HH:MM")}
            />
            <ReadonlyInfo
              name={"Modified at"}
              value={format(new Date(artist.modifiedAt), "dd.mm.yyyy, HH:MM")}
            />
            <ArtistInfoEditor artist={artist} setArtist={setArtist} />
            <ArtistDeleter artist={artist} setArtist={setArtist} />
          </Stack>
        </Grid>

        <Grid item xs={12} md={8} lg={4}>
          <Stack spacing={4}>
            <InfoCard title={"Tracks"} value={artist.tracksCount} />
            <InfoCard title={"Albums"} value={artist.albums.length} />
          </Stack>
        </Grid>

        <Grid item xs={12}>
          <AlbumsList albums={artist.albums} cover title publishedAt />
        </Grid>

        <Grid item xs={12}>
          <ArtistUsers artist={artist} />
        </Grid>
      </Grid>
    </>
  );
}
