import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { Genre } from "../types/Genre";
import { useNotifier } from "../../shared/hooks/Notifier.hook";
import Loading from "../../shared/components/Loading/Loading";
import NotFound from "../../shared/components/NotFound/NotFound";
import { Grid, Stack, Typography } from "@mui/material";
import ReadonlyInfo from "../../shared/components/InfoItem/ReadonlyInfo";
// @ts-ignore
import dateFormat from "dateformat";
import InfoCard from "../../shared/components/InfoCard/InfoCard";
import GenreInfoEditor from "../components/GenreInfoEditor/GenreInfoEditor";
import GenreDeleter from "../components/GenreDeleter/GenreDeleter";
import { useGenreService } from "../GenreService.hook";

export default function GenreDetailsPage() {
  const genreService = useGenreService();

  const { id } = useParams() as { id: string };
  const [genre, setGenre] = useState<Genre>();
  const [loading, setLoading] = useState(true);
  const { showError } = useNotifier();

  useEffect(() => {
    genreService
      .get(id)
      .then((genre) => {
        setGenre(genre);
      })
      .catch((error) => showError(error))
      .then(() => setLoading(false));
  }, [id]);

  if (loading) {
    return <Loading />;
  }

  if (!genre) {
    return <NotFound />;
  }

  return (
    <>
      <Typography variant={"h3"} component={"div"}>
        {genre.name}
      </Typography>

      <Grid
        container
        spacing={10}
        justifyContent={"space-around"}
        sx={{ mb: 10 }}
      >
        <Grid item xs={12} md={8} lg={4}>
          <Stack direction={"column"} spacing={3} alignItems={"end"}>
            <ReadonlyInfo name={"ID"} value={genre.id} />
            <ReadonlyInfo
              name={"Created at"}
              value={`${dateFormat(
                new Date(genre.createdAt),
                "dd.mm.yyyy, HH:MM"
              )}`}
            />
            <ReadonlyInfo
              name={"Modified at"}
              value={`${dateFormat(
                new Date(genre.modifiedAt),
                "dd.mm.yyyy, HH:MM"
              )}`}
            />
            <GenreInfoEditor
              id={id}
              dto={{ name: genre.name }}
              setDto={(dto) => {
                setGenre({ ...genre, ...dto });
              }}
            />
            <GenreDeleter genre={genre} setGenre={setGenre} />
          </Stack>
        </Grid>
        <Grid item xs={12} md={8} lg={4}>
          <Stack spacing={4}>
            <InfoCard title={"Tracks"} value={genre.tracksCount || 0} />
          </Stack>
        </Grid>
      </Grid>
    </>
  );
}
