import React from "react";
import { Button, Grid, Paper, Stack, Typography } from "@mui/material";
import ContrastTextField from "../../shared/components/ContrastTextField/ContrastTextField";
import { useNotifier } from "../../shared/hooks/Notifier.hook";
import { SubmitHandler, useForm } from "react-hook-form";
import { CreateGenreDto } from "../types/CreateGenreDto";
import { useNavigate } from "react-router-dom";
import { useGenreService } from "../GenreService.hook";

type Inputs = {
  name: string;
};

export default function CreateGenrePage() {
  const genreService = useGenreService();

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<Inputs>();
  const { showError, showSuccess } = useNotifier();
  const navigate = useNavigate();

  const onSubmit: SubmitHandler<Inputs> = async (data) => {
    try {
      // @ts-ignore
      const dto: CreateGenreDto = {
        name: data.name,
      };
      const genre = await genreService.save(dto);
      showSuccess("Genre created");
      navigate(`/admin/genres/${genre.id}`);
    } catch (e) {
      showError(e);
    }
  };

  return (
    <Grid container spacing={2} justifyContent={"center"}>
      <Grid item xs={12} md={10} lg={8}>
        <Paper sx={{ p: 4 }} elevation={10}>
          <Typography variant={"h3"} component={"div"} sx={{ mb: 4 }}>
            New genre
          </Typography>

          <Grid>
            <form onSubmit={handleSubmit(onSubmit)}>
              <Stack spacing={2} alignItems={"center"}>
                <ContrastTextField
                  error={!!errors.name && !!errors.name.message}
                  helperText={errors.name?.message}
                  label={"Name"}
                  fullWidth
                  {...register("name", {
                    required: { value: true, message: "Name required" },
                    minLength: { value: 2, message: "Min length - 4" },
                    maxLength: { value: 50, message: "Max length - 50" },
                  })}
                />
                <Button
                  type={"submit"}
                  variant="contained"
                  sx={{ borderRadius: "20px", px: 4, width: "200px" }}
                >
                  Create
                </Button>
              </Stack>
            </form>
          </Grid>
        </Paper>
      </Grid>
    </Grid>
  );
}
