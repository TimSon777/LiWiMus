import React, {useEffect, useState} from "react";
import {Link, Stack} from "@mui/material";
import {Link as RouterLink} from "react-router-dom";
import {Track} from "../../types/Track";
import TrackService from "../../Track.service";
import {useNotifier} from "../../../shared/hooks/Notifier.hook";

type Props = {
  track: Track;
};

export default function TrackArtists({ track }: Props) {
  const [loading, setLoading] = useState<boolean>(true);
  const { showError } = useNotifier();

  useEffect(() => {
    setLoading(true);
    TrackService.getArtists(track)
      .then((artists) => (track.artists = artists))
      .catch(showError)
      .then(() => setLoading(false));
  }, []);

  if (loading || !track.artists) {
    return <></>;
  }

  return (
    <Stack direction={"row"} spacing={1} divider={<span>&</span>}>
      {track.artists.map((artist) => (
        <Link
          key={artist.id}
          component={RouterLink}
          to={`/admin/artists/${artist.id}`}
          underline="none"
          color={"secondary"}
        >
          {artist.name}
        </Link>
      ))}
    </Stack>
  );
}