import React, { useState } from "react";
import { Button, Grid, Paper, Stack, Typography } from "@mui/material";
import ContrastTextField from "../../shared/components/ContrastTextField/ContrastTextField";
import ImageEditor from "../../shared/components/ImageEditor/ImageEditor";
import { useNotifier } from "../../shared/hooks/Notifier.hook";
import albumPlaceholder from "../../shared/images/image-placeholder.png";
import {Controller, SubmitHandler, useForm} from "react-hook-form";
import { CreateAlbumDto } from "../types/CreateAlbumDto";
import { useNavigate } from "react-router-dom";
import {DatePicker, DateTimePicker} from "@mui/x-date-pickers";
import { useAlbumService } from "../AlbumService.hook";
import { useFileService } from "../../shared/hooks/FileService.hook";
import { format, parse } from "date-fns";

type Inputs = {
  title: string;
  publishedAt: Date | null,
  photoFlag: string;
};

export default function CreateAlbumPage() {
  const albumService = useAlbumService();
  const fileService = useFileService();
  const {
    register,
    handleSubmit,
    formState: { errors },
    clearErrors,
    setValue,
      control,
  } = useForm<Inputs>({
    defaultValues:{title:"", publishedAt: null, photoFlag:""}
  });

  const [photo, setPhoto] = useState<File>();
  const [photoBase64, setPhotoBase64] = useState<string>(albumPlaceholder);
  const { showError, showSuccess } = useNotifier();
  const navigate = useNavigate();

  const onSubmit: SubmitHandler<Inputs> = async (data) => {
    try {
      // @ts-ignore
      const coverLocation = await fileService.save(photo);
      
      const dto: CreateAlbumDto = {
        title: data.title,
        publishedAt: format(data.publishedAt as Date, "yyyy-MM-dd"),
        coverLocation,
      };
      const album = await albumService.save(dto);
      showSuccess("Album created");
      navigate(`/admin/albums/${album.id}`);
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
            New album
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
                    error={!!errors.title && !!errors.title.message}
                    helperText={errors.title?.message}
                    label={"Title"}
                    fullWidth
                    {...register("title", {
                      required: { value: true, message: "Title required" },
                      minLength: { value: 1, message: "Min length - 1" },
                      maxLength: { value: 50, message: "Max length - 50" },
                    })}
                  />
                  <Controller
                      name={"publishedAt"}
                      control={control}
                      rules={{
                        required: { value: true, message: "Required" },
                      }}
                      
                      render={({ field }) => (
                          <DatePicker
                              maxDate={new Date()}
                              mask={"__.__.____"}
                              label={"Published at"}
                              InputProps={{
                                error: !!errors.publishedAt,
                              }}
                              renderInput={(params) => (
                                  <ContrastTextField
                                      fullWidth
                                      helperText={errors.publishedAt?.message}
                                      {...params}
                                  />
                              )}
                              {...field}
                          />
                      )}
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
