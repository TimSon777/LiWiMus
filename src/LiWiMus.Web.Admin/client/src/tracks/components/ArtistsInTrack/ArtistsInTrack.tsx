import React from "react";
import { Track } from "../../types/Track";
import { Artist } from "../../../artists/types/Artist";
import { useNotifier } from "../../../shared/hooks/Notifier.hook";
import TrackService from "../../../tracks/Track.service";
import ArtistsList from "../../../artists/components/ArtistsList/ArtistsList";
import AlertDialog from "../../../shared/components/AlertDialog/AlertDialog";

type Props = {
  track: Track;
  setTrack: (track: Track) => void;
};

export default function ArtistsInTrack({ track, setTrack }: Props) {
  const { showError, showSuccess } = useNotifier();

  const removeArtist = async (artist: Artist) => {
    try {
      await TrackService.removeArtist(track, artist);
      setTrack({
        ...track,
        artists: track.artists.filter((a) => a.id !== artist.id),
      });
      showSuccess("Artist removed from track");
    } catch (e) {
      showError(e);
    }
  };

  if (track.artists.length === 0) {
    return <></>;
  }

  return (
    <ArtistsList
      artists={track.artists}
      photo
      name
      action={(artist) => (
        <AlertDialog
          onAgree={() => removeArtist(artist)}
          title={"Remove artist from track?"}
          text={"You can add it back later"}
          agreeText={"Remove"}
          disagreeText={"Cancel"}
          buttonText={"Remove"}
        />
      )}
    />
  );
}
