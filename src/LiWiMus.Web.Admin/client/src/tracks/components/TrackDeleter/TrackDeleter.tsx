import React from "react";
import { Track } from "../../types/Track";
import { useNavigate } from "react-router-dom";
import { useNotifier } from "../../../shared/hooks/Notifier.hook";
import Deleter from "../../../shared/components/Deleter/Deleter";
import { useTrackService } from "../../TrackService.hook";

type Props = {
  track: Track;
  setTrack: (track: Track | undefined) => void;
};

export default function TrackDeleter({ track, setTrack }: Props) {
  const trackService = useTrackService();

  const navigate = useNavigate();
  const { showSuccess, showError } = useNotifier();

  const deleteHandler = async () => {
    try {
      await trackService.remove(track);
      setTrack(undefined);
      showSuccess("Track deleted");

      navigate("/admin/tracks");
    } catch (error) {
      // @ts-ignore
      showError(error);
    }
  };

  return <Deleter itemName={track.name} deleteHandler={deleteHandler} />;
}
