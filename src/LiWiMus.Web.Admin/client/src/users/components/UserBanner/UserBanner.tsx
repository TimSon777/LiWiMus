import React, { useState } from "react";
import { User } from "../../types/User";
import { Action } from "../../../shared/types/Action";
import { Button, Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle } from "@mui/material";
import { Controller, SubmitHandler, useForm } from "react-hook-form";
import { DateTimePicker } from "@mui/x-date-pickers";
import ContrastTextField from "../../../shared/components/ContrastTextField/ContrastTextField";
import { useNotifier } from "../../../shared/hooks/Notifier.hook";
import { isPast } from "date-fns";
import { useUserService } from "../../UserService.hook";

type Inputs = {
  end: Date | null;
};

type Props = {
  user: User;
  setUser: Action<User>;
};

export default function UserBanner({ user, setUser }: Props) {
  const userService = useUserService();

  const { showError, showSuccess } = useNotifier();
  const [open, setOpen] = useState(false);

  const {
    handleSubmit,
    formState: { errors },
    control,
  } = useForm<Inputs>({
    defaultValues: { end: new Date(2099, 11, 31) },
  });

  const handleClickOpen = () => {
    setOpen(true);
    console.log(user);
  };

  const handleClose = () => {
    setOpen(false);
  };

  const banUser: SubmitHandler<Inputs> = async (data) => {
    try {
      const result = await userService.lockOut(user, data.end!);
      setUser(result);
      showSuccess("User banned");
      handleClose();
    } catch (e) {
      showError(e);
    }
  };

  const unbanUser = async () => {
    try {
      const result = await userService.lockOut(user, new Date());
      setUser(result);
      showSuccess("User unbanned");
    } catch (e) {
      showError(e);
    }
  };

  return !user.lockoutEnd || isPast(user.lockoutEnd) ? (
    <>
      <Dialog
        open={open}
        onClose={handleClose}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
      >
        <DialogTitle>Ban user</DialogTitle>
        <DialogContent>
          <DialogContentText sx={{ mb: 3 }}>
            Select an end date for the ban
          </DialogContentText>

          <Controller
            name={"end"}
            control={control}
            rules={{
              required: { value: true, message: "Required" },
              validate: (value) =>
                !value || value > new Date() ? true : "Must be in future",
            }}
            render={({ field }) => (
              <DateTimePicker
                label={"End"}
                InputProps={{
                  error: !!errors.end,
                }}
                minDateTime={new Date()}
                renderInput={(params) => (
                  <ContrastTextField
                    helperText={errors.end?.message}
                    {...params}
                  />
                )}
                {...field}
              />
            )}
          />
        </DialogContent>
        <DialogActions sx={{ justifyContent: "center" }}>
          <Button
            onClick={handleClose}
            color={"secondary"}
            sx={{ borderRadius: "20px", px: 4 }}
          >
            Cancel
          </Button>
          <Button
            onClick={handleSubmit(banUser)}
            autoFocus
            variant={"contained"}
            sx={{ borderRadius: "20px", px: 4 }}
          >
            Ok
          </Button>
        </DialogActions>
      </Dialog>

      <Button
        variant="contained"
        color={"error"}
        sx={{ borderRadius: "20px", px: 4 }}
        onClick={handleClickOpen}
      >
        Ban
      </Button>
    </>
  ) : (
    <Button
      variant="contained"
      sx={{ borderRadius: "20px", px: 4 }}
      onClick={unbanUser}
    >
      Unban
    </Button>
  );
}
