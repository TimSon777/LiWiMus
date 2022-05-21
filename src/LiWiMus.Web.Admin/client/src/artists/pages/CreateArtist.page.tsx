import React, { useState } from "react";
import { Button, Grid, Paper, Stack, Typography } from "@mui/material";
import ContrastTextField from "../../shared/components/ContrastTextField/ContrastTextField";
import ImageEditor from "../../shared/components/ImageEditor/ImageEditor";
import { useNotifier } from "../../shared/hooks/Notifier.hook";
import artistPlaceholder from "../../shared/images/avatar-placeholder.jpg";
import { SubmitHandler, useForm } from "react-hook-form";
import ArtistService from "../Artist.service";
import FileService from "../../shared/services/File.service";
import { CreateArtistDto } from "../types/CreateArtistDto";
import { useNavigate } from "react-router-dom";

type Inputs = {
  name: string;
  about: string;
  photoFlag: string;
};

export default function CreateArtistPage() {
  const {
    register,
    handleSubmit,
    formState: { errors },
    setValue,
    clearErrors,
  } = useForm<Inputs>();

  const [photo, setPhoto] = useState<File>();
  const [photoBase64, setPhotoBase64] = useState<string>(artistPlaceholder);
  const { showError, showSuccess } = useNotifier();
  const navigate = useNavigate();

  const onSubmit: SubmitHandler<Inputs> = async (data) => {
    try {
      // @ts-ignore
      const photoLocation = await FileService.save(photo);
      const dto: CreateArtistDto = {
        name: data.name,
        about: data.about,
        photoLocation,
      };
      const artist = await ArtistService.save(dto);
      showSuccess("Artist created");
      navigate(`/admin/artists/${artist.id}`);
    } catch (e) {
      showError(e);
    }
  };

  const changePhotoHandler = async (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    const input = event.target;
    if (!input.files || !input.files[0]) {
      setValue("photoFlag", "");
      return;
    }
    const cover = input.files[0];

    const reader = new FileReader();
    reader.readAsDataURL(cover);
    reader.onload = () => {
      setPhotoBase64(reader.result as string);
    };

    reader.onerror = (error) => {
      showError(error);
    };

    setPhoto(cover);
    setValue("photoFlag", ".");
    clearErrors("photoFlag");
  };

  return (
    <Grid container spacing={2} justifyContent={"center"}>
      <Grid item xs={12} md={10} lg={8}>
        <Paper sx={{ p: 4 }} elevation={10}>
          <Typography variant={"h3"} component={"div"} sx={{ mb: 4 }}>
            New artist
          </Typography>

          <Grid container spacing={2}>
            <Grid
              item
              xs={12}
              md={6}
              sx={{
                display: "flex",
                flexDirection: "column",
                alignItems: "center",
              }}
            >
              <ImageEditor
                src={photoBase64}
                width={250}
                handler1={(input) => input.click()}
                onChange={changePhotoHandler}
              />
              {errors.photoFlag && errors.photoFlag.message && (
                <Typography color={"error"} sx={{ mt: 2 }}>
                  {errors.photoFlag.message}
                </Typography>
              )}
            </Grid>
            <Grid item xs={12} md={6}>
              <form onSubmit={handleSubmit(onSubmit)}>
                <input
                  type="text"
                  hidden
                  {...register("photoFlag", {
                    required: { value: true, message: "Photo required" },
                  })}
                />

                <Stack spacing={2} alignItems={"center"}>
                  <ContrastTextField
                    error={!!errors.name && !!errors.name.message}
                    helperText={errors.name?.message}
                    label={"Name"}
                    fullWidth
                    {...register("name", {
                      required: { value: true, message: "Name required" },
                      minLength: { value: 5, message: "Min length - 5" },
                      maxLength: { value: 50, message: "Max length - 50" },
                    })}
                  />
                  <ContrastTextField
                    error={!!errors.about && !!errors.about.message}
                    helperText={errors.about?.message}
                    label={"About"}
                    fullWidth
                    multiline
                    {...register("about", {
                      required: {
                        value: true,
                        message: "Description required",
                      },
                      maxLength: { value: 500, message: "Max length - 500" },
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
          </Grid>
        </Paper>
      </Grid>
    </Grid>
  );
}
