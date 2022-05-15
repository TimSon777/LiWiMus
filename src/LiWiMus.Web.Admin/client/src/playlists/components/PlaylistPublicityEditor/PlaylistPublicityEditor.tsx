import React from "react";
import { Button } from "@mui/material";
import PlaylistService from "../../Playlist.service";
import { useNotifier } from "../../../shared/hooks/Notifier.hook";

type Inputs = {
  isPublic: boolean;
};

type Props = {
  id: string;
  dto: Inputs;
  setDto: (dto: Inputs) => void;
};

export default function PlaylistPublicityEditor({ id, dto, setDto }: Props) {
  const { showError, showSuccess } = useNotifier();

  const publicityHandler = async () => {
    const req = { id, isPublic: !dto.isPublic };
    try {
      const data = (await PlaylistService.update(req)) as Inputs;
      setDto(data);
      showSuccess(`Playlist is ${data.isPublic ? "public" : "private"}`);
    } catch (error) {
      // @ts-ignore
      showError(error);
    }
  };

  return (
    <Button
      variant="contained"
      color={"error"}
      sx={{ borderRadius: "20px", px: 4 }}
      onClick={publicityHandler}
    >
      Make {dto.isPublic ? "private" : "public"}
    </Button>
  );
}
