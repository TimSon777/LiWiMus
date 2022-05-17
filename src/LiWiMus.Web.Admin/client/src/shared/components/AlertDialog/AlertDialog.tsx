import React, { useState } from "react";
import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
} from "@mui/material";

type Props = {
  onAgree: () => void;
  onDisagree?: () => void;
  onClose?: () => void;
  title?: string;
  text?: string;
  disagreeText?: string;
  agreeText?: string;
  buttonText?: string;
};

export default function AlertDialog({
  onAgree,
  onDisagree,
  onClose,
  title = "Are you sure?",
  text = "This action cannot be undone.",
  disagreeText = "Disagree",
  agreeText = "Agree",
  buttonText = "Alert",
}: Props) {
  const [open, setOpen] = useState(false);

  const handleClickOpen = () => {
    setOpen(true);
  };

  const handleClose = () => {
    if (onClose) {
      onClose();
    }
    setOpen(false);
  };

  const handleDisagree = () => {
    if (onDisagree) {
      onDisagree();
    }
    handleClose();
  };

  const handleAgree = () => {
    onAgree();
    handleClose();
  };

  return (
    <>
      <Dialog
        open={open}
        onClose={handleClose}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
      >
        <DialogTitle>{title}</DialogTitle>
        <DialogContent>
          <DialogContentText>{text}</DialogContentText>
        </DialogContent>
        <DialogActions sx={{ justifyContent: "center" }}>
          <Button
            onClick={handleDisagree}
            color={"secondary"}
            sx={{ borderRadius: "20px", px: 4 }}
          >
            {disagreeText}
          </Button>
          <Button
            onClick={handleAgree}
            autoFocus
            variant={"contained"}
            sx={{ borderRadius: "20px", px: 4 }}
          >
            {agreeText}
          </Button>
        </DialogActions>
      </Dialog>

      <Button
        variant="contained"
        color={"error"}
        sx={{ borderRadius: "20px", px: 4 }}
        onClick={handleClickOpen}
      >
        {buttonText}
      </Button>
    </>
  );
}
