import React from "react";
import { useNotifier } from "../../../shared/hooks/Notifier.hook";
import { SubmitHandler, useForm } from "react-hook-form";
import { Box, Button, Stack } from "@mui/material";
import ContrastTextField from "../../../shared/components/ContrastTextField/ContrastTextField";
import { useGenreService } from "../../GenreService.hook";

type Inputs = {
  name: string;
};

type Props = {
  id: string;
  dto: Inputs;
  setDto: (dto: Inputs) => void;
};

export default function GenreInfoEditor({ id, dto, setDto }: Props) {
  const genreService = useGenreService();

  const { showSuccess, showError } = useNotifier();

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
      const req = { ...data, id: +id };
      const response = (await genreService.update(req)) as Inputs;
      showSuccess("Info updated");
      setDto(response);
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
            required: { value: true, message: "Enter genre name" },
            maxLength: {
              value: 50,
              message: "Maximum length - 50 characters",
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
