import React from "react";
import { PaginatedData } from "../../../shared/types/PaginatedData";
import { Album } from "../../types/Album";
import { useNotifier } from "../../../shared/hooks/Notifier.hook";
import ArtistService from "../../../artists/Artist.service";
import AlbumService from "../../../albums/Album.service";
import Loading from "../../../shared/components/Loading/Loading";
import InfiniteScroll from "react-infinite-scroll-component";
import ArtistsList from "../../../artists/components/ArtistsList/ArtistsList";
import { Button } from "@mui/material";
import { Artist } from "../../../artists/types/Artist";

type Props = {
  album: Album;
  setAlbum: (album: Album) => void;
  artists: PaginatedData<Artist>;
  setArtists: (artists: PaginatedData<Artist>) => void;
  artistsInAlbum: PaginatedData<Artist>;
  setArtistsInAlbum: (artists: PaginatedData<Artist>) => void;
  filter: string;
  loading: boolean;
};

export default function ArtistsInSearch({
  artists,
  setArtists,
  setArtistsInAlbum,
  album,
  artistsInAlbum,
  setAlbum,
  filter,
  loading,
}: Props) {
  const { showError, showSuccess } = useNotifier();

  const fetchMore = async () => {
    try {
      const newArtists = await ArtistService.getArtists({
        filters: [
          { columnName: "name", operator: "cnt", value: filter },
          {
            columnName: "id",
            operator: "-in",
            value: [0, ...artistsInAlbum.data.map((artist) => artist.id)],
          },
        ],
        page: {
          pageNumber: artists.actualPage + 1,
          numberOfElementsOnPage: 10,
        },
      });
      setArtists({
        ...newArtists,
        data: [...artists.data, ...newArtists.data],
      });
    } catch (e) {
      showError(e);
    }
  };

  const addArtist = async (artist: Artist) => {
    try {
      await AlbumService.addArtist(album, artist);
      setArtistsInAlbum({
        ...artistsInAlbum,
        totalItems: artistsInAlbum.totalItems + 1,
        data: [...artistsInAlbum.data, artist],
      });
      setArtists({
        ...artists,
        totalItems: artists.totalItems - 1,
        data: artists.data.filter((t) => t.id !== artist.id),
      });
      setAlbum({ ...album, artists: [...album.artists, artist] });
      showSuccess("Artist added to album");
    } catch (e) {
      showError(e);
    }
  };

  if (loading) {
    return <Loading />;
  }

  if (artists.data.length === 0) {
    return <></>;
  }

  return (
    <InfiniteScroll
      dataLength={artists.data.length}
      hasMore={artists.hasMore}
      loader={<Loading />}
      next={fetchMore}
    >
      <ArtistsList
        artists={artists.data}
        photo
        name
        about
        action={(artist) => (
          <Button
            variant="outlined"
            color={"secondary"}
            sx={{ borderRadius: "20px", px: 4 }}
            onClick={() => addArtist(artist)}
          >
            Add
          </Button>
        )}
      />
    </InfiniteScroll>
  );
}
