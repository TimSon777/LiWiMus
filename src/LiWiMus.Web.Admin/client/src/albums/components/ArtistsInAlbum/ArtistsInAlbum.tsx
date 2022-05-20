import React, { useEffect, useState } from "react";
import { Album } from "../../types/Album";
import { PaginatedData } from "../../../shared/types/PaginatedData";
import { useNotifier } from "../../../shared/hooks/Notifier.hook";
import Loading from "../../../shared/components/Loading/Loading";
import InfiniteScroll from "react-infinite-scroll-component";
import AlertDialog from "../../../shared/components/AlertDialog/AlertDialog";
import AlbumService from "../../Album.service";
import { Artist } from "../../../artists/types/Artist";
import ArtistsList from "../../../artists/components/ArtistsList/ArtistsList";

type Props = {
  album: Album;
  setAlbum: (album: Album) => void;
  artists: PaginatedData<Artist>;
  setArtists: (artists: PaginatedData<Artist>) => void;
};

export default function ArtistsInAlbum({
  album,
  setAlbum,
  artists,
  setArtists,
}: Props) {
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const { showError, showSuccess } = useNotifier();

  const fetchMore = async () => {
    try {
      const newArtists = await AlbumService.getArtists(
        album,
        artists.actualPage + 1,
        10
      );
      setArtists({
        ...newArtists,
        data: [...artists.data, ...newArtists.data],
        hasMore: newArtists.actualPage < newArtists.totalPages,
      });
    } catch (e) {
      showError(e);
    }
  };

  useEffect(() => {
    if (artists.hasMore) {
      setIsLoading(true);
      fetchMore().then(() => {
        setIsLoading(false);
      });
    } else {
      setIsLoading(false);
    }
  }, []);

  const removeArtist = async (artist: Artist) => {
    try {
      await AlbumService.removeArtist(album, artist);
      setArtists({
        ...artists,
        data: artists.data.filter((t) => t.id !== artist.id),
        totalItems: artists.totalItems - 1,
      });
      setAlbum({
        ...album,
        artists: album.artists.filter((a) => a.id !== artist.id),
      });
      showSuccess("Artist removed from album");
    } catch (e) {
      showError(e);
    }
  };

  if (isLoading) {
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
          <AlertDialog
            onAgree={() => removeArtist(artist)}
            title={"Remove artist from album?"}
            text={"You can add it back later"}
            agreeText={"Remove"}
            disagreeText={"Cancel"}
            buttonText={"Remove"}
          />
        )}
      />
    </InfiniteScroll>
  );
}
