import React from "react";
import { Artist } from "../../types/Artist";
import { useNotifier } from "../../../shared/hooks/Notifier.hook";
import { Controller, SubmitHandler, useForm } from "react-hook-form";
import { UpdateArtistDto } from "../../types/UpdateArtistDto";
import ArtistService from "../../Artist.service";
import { Box, Button, Stack } from "@mui/material";
import ContrastTextField from "../../../shared/components/ContrastTextField/ContrastTextField";

type Inputs = {
  name: string;
  about: string;
};

type Props = {
  artist: Artist;
  setArtist: (artist: Artist) => void;
};

export default function ArtistInfoEditor({ artist, setArtist }: Props) {
  const { showSuccess, showError } = useNotifier();
  let defaultValues = { about: artist.about, name: artist.name };
  const {
    register,
    handleSubmit,
    watch,
    formState: { errors },
    reset,
    control,
  } = useForm<Inputs>({ defaultValues });

  const actual = watch();
  const isChanged = JSON.stringify(actual) !== JSON.stringify(defaultValues);

  const rollbackHandler = () => {
    reset(defaultValues);
  };

  const saveHandler: SubmitHandler<Inputs> = async (data) => {
    if (!isChanged) {
      return;
    }
    try {
      const req: UpdateArtistDto = {
        id: +artist.id,
        name: data.name,
        about: data.about,
      };
      const response = await ArtistService.update(req);
      showSuccess("Info updated");
      setArtist({ ...artist, ...response });
    } catch (error) {
      // @ts-ignore
      showError(error);
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
            required: true,
            maxLength: 50,
            minLength: 5,
          })}
        />

        <Controller
          name="about"
          control={control}
          rules={{
            required: {
              value: true,
              message: "Description required",
            },
            maxLength: {
              value: 500,
              message: "Max length - 500",
            },
          }}
          render={({ field }) => (
            <ContrastTextField
              error={!!errors.about && !!errors.about.message}
              helperText={errors.about?.message}
              label={"About"}
              multiline
              {...field}
            />
          )}
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
