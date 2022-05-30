import React from "react";
import { Track } from "../../types/Track";
import { Button } from "@mui/material";
import { useNotifier } from "../../../shared/hooks/Notifier.hook";
import { useTrackService } from "../../TrackService.hook";

type Props = {
  track: Track;
  setTrack: (track: Track) => void;
};

export default function TrackFileEditor({ track, setTrack }: Props) {
  const trackService = useTrackService();

  const { showError, showSuccess } = useNotifier();

  const changeHandler = async (event: React.ChangeEvent<HTMLInputElement>) => {
    try {
      if (!event.target.files || !event.target.files[0]) {
        return;
      }
      const file = event.target.files[0];
      const updated = await trackService.updateFile(track, file);
      setTrack({ ...track, ...updated });
      showSuccess("Audio file updated");
    } catch (e) {
      showError(e);
    }
  };

  return (
    <Button
      component={"label"}
      variant="contained"
      sx={{ borderRadius: "20px", px: 4 }}
    >
      Upload file
      <input type={"file"} hidden onChange={changeHandler} accept={"audio/*"} />
    </Button>
  );
}
