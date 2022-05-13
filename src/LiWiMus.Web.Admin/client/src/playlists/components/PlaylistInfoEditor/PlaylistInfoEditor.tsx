import React from "react";
import { Box, Button, Stack } from "@mui/material";
import ContrastTextField from "../../../shared/components/ContrastTextField/ContrastTextField";
import { SubmitHandler, useForm } from "react-hook-form";
import axios from "../../../shared/services/Axios";
import { useSnackbar } from "notistack";

type Inputs = {
  name: string;
};

type Props = {
  id: string;
  dto: Inputs;
  setDto: (dto: Inputs) => void;
};

export default function PlaylistInfoEditor({ id, dto, setDto }: Props) {
  const { enqueueSnackbar } = useSnackbar();

  const {
    register,
    handleSubmit,
    watch,
    formState: { errors },
    reset,
  } = useForm<Inputs>({ defaultValues: dto });

  const actual = watch();
  const isChanged = JSON.stringify(actual) !== JSON.stringify(dto);

  const rollbackHandler = () => {
    reset(dto);
  };

  const saveHandler: SubmitHandler<Inputs> = async (data) => {
    if (JSON.stringify(data) === JSON.stringify(dto)) {
      return;
    }
    try {
      const req = { ...data, id };
      const response = (await axios.patch("/playlists", req)).data as Inputs;
      enqueueSnackbar("Info updated", { variant: "success" });
      setDto(response);
    } catch (error) {
      // @ts-ignore
      enqueueSnackbar(error.message, { variant: "error" });
    }
  };

  return (
    <Box sx={{ display: "flex", flexDirection: "column", width: "100%" }}>
      <Stack direction={"column"} spacing={2}>
        <ContrastTextField
          error={!!errors.name && !!errors.name.message}
          helperText={errors.name?.message}
          label="Name"
          InputLabelProps={{
            shrink: true,
          }}
          variant="outlined"
          fullWidth
          {...register("name", {
            required: { value: true, message: "Enter playlist name" },
            maxLength: {
              value: 100,
              message: "Maximum length - 100 characters",
            },
          })}
        />
        {isChanged && (
          <Stack direction={"row"} sx={{ alignSelf: "flex-end" }} spacing={2}>
            <Button
              onClick={rollbackHandler}
              variant="text"
              color={"secondary"}
              sx={{ borderRadius: "20px", px: 4 }}
            >
              Rollback
            </Button>
            <Button
              onClick={handleSubmit(saveHandler)}
              variant="contained"
              sx={{ borderRadius: "20px", px: 4 }}
            >
              Save
            </Button>
          </Stack>
        )}
      </Stack>
    </Box>
  );
}
