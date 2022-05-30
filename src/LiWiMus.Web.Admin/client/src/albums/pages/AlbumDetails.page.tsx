import React, { useEffect, useState } from "react";
// @ts-ignore
import dateFormat from "dateformat";
import { useParams } from "react-router-dom";
import { useNotifier } from "../../shared/hooks/Notifier.hook";
import Loading from "../../shared/components/Loading/Loading";
import NotFound from "../../shared/components/NotFound/NotFound";
import { Album } from "../types/Album";
import { Grid, Stack, Typography } from "@mui/material";
import AlbumImageEditor from "../components/AlbumImageEditor/AlbumImageEditor";
import ReadonlyInfo from "../../shared/components/InfoItem/ReadonlyInfo";
import InfoCard from "../../shared/components/InfoCard/InfoCard";
import AlbumInfoEditor from "../components/AlbumInfoEditor/AlbumInfoEditor";
import ArtistsLinks from "../../artists/components/ArtistsLinks/ArtistsLinks";
import { format, parse } from "date-fns";
import AlbumDeleter from "../components/AlbumDeleter/AlbumDeleter";
import AlbumTracks from "../components/AlbumTracks/AlbumTracks";
import AlbumArtists from "../components/AlbumArtists/AlbumArtists";
import { useAlbumService } from "../AlbumService.hook";
import { useFileService } from "../../shared/hooks/FileService.hook";

export default function AlbumDetailsPage() {
  const { id } = useParams() as { id: string };
  const [album, setAlbum] = useState<Album>();
  const [albumCover, setAlbumCover] = useState<string>("");
  const [loading, setLoading] = useState(true);
  const { showError } = useNotifier();
  const albumService = useAlbumService();
  const fileService = useFileService();

  const setAlbumWithCover = (newAlbum: Album) => {
    const coverLocation = fileService.getLocation(newAlbum.coverLocation);
    setAlbum(newAlbum);
    setAlbumCover(coverLocation);
  };

  useEffect(() => {
    albumService
      .get(id)
      .then((album) => {
        setAlbumWithCover(album);
      })
      .catch((error) => showError(error))
      .then(() => setLoading(false));
  }, [id]);

  if (loading) {
    return <Loading />;
  }

  if (!album) {
    return <NotFound />;
  }

  return (
    <>
      <Typography variant={"h3"} component={"div"}>
        {album.title}
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
          <AlbumImageEditor
            album={album}
            coverSrc={albumCover}
            setAlbumWithCover={setAlbumWithCover}
          />
        </Grid>

        <Grid item xs={12} md={8} lg={4}>
          <Stack direction={"column"} spacing={3} alignItems={"end"}>
            <ReadonlyInfo name={"ID"} value={album.id} />
            <ReadonlyInfo
              name={"Created at"}
              value={`${dateFormat(
                new Date(album.createdAt),
                "dd.mm.yyyy, HH:MM"
              )}`}
            />
            <ReadonlyInfo
              name={"Modified at"}
              value={`${dateFormat(
                new Date(album.modifiedAt),
                "dd.mm.yyyy, HH:MM"
              )}`}
            />
            <ReadonlyInfo
              name={"Artists"}
              value={<ArtistsLinks artists={album.artists} />}
            />
            <AlbumInfoEditor
              id={id}
              dto={{
                title: album.title,
                publishedAt: parse(album.publishedAt, "yyyy-MM-dd", new Date()),
              }}
              setDto={(dto) => {
                setAlbum({
                  ...album,
                  title: dto.title,
                  publishedAt: format(dto.publishedAt, "yyyy-MM-dd"),
                });
              }}
            />
            <AlbumDeleter album={album} setAlbum={setAlbum} />
          </Stack>
        </Grid>

        <Grid item xs={12} md={8} lg={4}>
          <Stack spacing={4}>
            <InfoCard title={"Tracks"} value={album.tracksCount} />
            <InfoCard title={"Listeners"} value={album.listenersCount} />
          </Stack>
        </Grid>

        <Grid item xs={12}>
          <AlbumTracks album={album} />
        </Grid>

        <Grid item xs={12}>
          <AlbumArtists album={album} setAlbum={setAlbum} />
        </Grid>
      </Grid>
    </>
  );
}
