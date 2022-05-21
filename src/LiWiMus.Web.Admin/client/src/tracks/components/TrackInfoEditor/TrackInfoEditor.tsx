import React from "react";
import { Track } from "../../types/Track";
import { Button, Stack } from "@mui/material";
import ReadonlyInfo from "../../../shared/components/InfoItem/ReadonlyInfo";
import formatDuration from "format-duration";
import {
  addHours,
  formatDistanceToNow,
  formatISO,
  parse,
  parseISO,
} from "date-fns";
import { useNotifier } from "../../../shared/hooks/Notifier.hook";
import { Controller, SubmitHandler, useForm } from "react-hook-form";
import { UpdateTrackDto } from "../../types/UpdateTrackDto";
import TrackService from "../../Track.service";
import ContrastTextField from "../../../shared/components/ContrastTextField/ContrastTextField";
import { DatePicker } from "@mui/x-date-pickers";

type Inputs = {
  name: string;
  publishedAt: Date;
};

type Props = {
  track: Track;
  setTrack: (track: Track | undefined) => void;
};

export default function TrackInfoEditor({ track, setTrack }: Props) {
  const defaultInputs: Inputs = {
    name: track.name,
    publishedAt: parse(track.publishedAt, "yyyy-MM-dd", new Date()),
  };
  const { showSuccess, showError } = useNotifier();
  const {
    register,
    handleSubmit,
    watch,
    formState: { errors },
    reset,
    control,
  } = useForm<Inputs>({ defaultValues: defaultInputs });

  const actual = watch();
  const isChanged = JSON.stringify(actual) !== JSON.stringify(defaultInputs);

  const rollbackHandler = () => {
    reset(defaultInputs);
  };

  const saveHandler: SubmitHandler<Inputs> = async (data) => {
    if (!isChanged) {
      return;
    }
    try {
      const req: UpdateTrackDto = {
        id: +track.id,
        name: data.name,
        publishedAt: formatISO(data.publishedAt),
      };
      const response = await TrackService.update(req);
      showSuccess("Info updated");
      setTrack({ ...track, ...response });
    } catch (error) {
      // @ts-ignore
      showError(error);
    }
  };

  return (
    <form>
      <Stack spacing={3}>
        <ReadonlyInfo name={"ID"} value={track.id} />
        <ReadonlyInfo
          name={"Duration"}
          value={formatDuration(track.duration * 1000)}
        />
        <ReadonlyInfo
          name={"Created"}
          value={formatDistanceToNow(addHours(parseISO(track.createdAt), 3), {
            addSuffix: true,
          })}
        />
        <ReadonlyInfo
          name={"Modified"}
          value={formatDistanceToNow(addHours(parseISO(track.modifiedAt), 3), {
            addSuffix: true,
          })}
        />
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
          name={"publishedAt"}
          control={control}
          rules={{ required: true }}
          render={({ field }) => (
            <DatePicker
              label={"Published at"}
              mask={"__.__.____"}
              renderInput={(params) => (
                <ContrastTextField
                  InputLabelProps={{
                    shrink: true,
                  }}
                  error={!!errors.publishedAt && !!errors.publishedAt.message}
                  helperText={errors.publishedAt?.message}
                  {...params}
                />
              )}
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
    </form>
  );
}
