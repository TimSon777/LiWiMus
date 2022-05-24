import React from "react";
import { User } from "../../types/User";
import {
  Button,
  FormControl,
  InputLabel,
  MenuItem,
  Select,
  Stack,
} from "@mui/material";
import { formatISO } from "date-fns";
import { useNotifier } from "../../../shared/hooks/Notifier.hook";
import { Controller, SubmitHandler, useForm } from "react-hook-form";
import { UpdateUserDto } from "../../types/UpdateUserDto";
import UserService from "../../User.service";
import ContrastTextField from "../../../shared/components/ContrastTextField/ContrastTextField";
import { DatePicker } from "@mui/x-date-pickers";
import { Gender } from "../../types/Gender";

type Props = {
  user: User;
  setUser: (user: User) => void;
};

type Inputs = {
  firstName: string;
  secondName: string;
  patronymic: string;
  gender: Gender | "";
  birthDate: Date | null;
};

export default function UserInfoEditor({ user, setUser }: Props) {
  const defaultInputs: Inputs = {
    firstName: user.firstName ?? "",
    gender: user.gender ?? "",
    birthDate: user.birthDate ?? null,
    patronymic: user.patronymic ?? "",
    secondName: user.secondName ?? "",
  };
  const { showSuccess, showError } = useNotifier();
  const {
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
      const req: UpdateUserDto = {
        id: +user.id,
        // @ts-ignore
        birthDate: data.birthDate ? formatISO(data.birthDate) : undefined,
        gender: data.gender ? data.gender : undefined,
        firstName: data.firstName ? data.firstName : undefined,
        patronymic: data.patronymic ? data.patronymic : undefined,
        secondName: data.secondName ? data.secondName : undefined,
      };
      const response = await UserService.update(req);
      showSuccess("Info updated");
      setUser({ ...user, ...response });
    } catch (error) {
      // @ts-ignore
      showError(error);
    }
  };

  return (
    <form>
      <Stack spacing={3}>
        <Controller
          name="firstName"
          control={control}
          rules={{
            required: { value: !!user.firstName, message: "Required" },
            maxLength: { value: 50, message: "Max length - 50" },
          }}
          render={({ field }) => (
            <ContrastTextField
              error={!!errors.firstName && !!errors.firstName.message}
              helperText={errors.firstName?.message}
              label={"First name"}
              {...field}
            />
          )}
        />
        <Controller
          name="secondName"
          control={control}
          rules={{
            required: { value: !!user.secondName, message: "Required" },
            maxLength: { value: 50, message: "Max length - 50" },
          }}
          render={({ field }) => (
            <ContrastTextField
              error={!!errors.secondName && !!errors.secondName.message}
              helperText={errors.secondName?.message}
              label={"Second name"}
              {...field}
            />
          )}
        />
        <Controller
          name="patronymic"
          control={control}
          rules={{
            required: { value: !!user.patronymic, message: "Required" },
            maxLength: { value: 50, message: "Max length - 50" },
          }}
          render={({ field }) => (
            <ContrastTextField
              error={!!errors.patronymic && !!errors.patronymic.message}
              helperText={errors.patronymic?.message}
              label={"Patronymic"}
              {...field}
            />
          )}
        />
        <Controller
          name="gender"
          control={control}
          rules={{
            required: { value: !!user.gender, message: "Required" },
          }}
          render={({ field }) => (
            <FormControl fullWidth>
              <InputLabel id="demo-simple-select-label">Gender</InputLabel>
              <Select
                value={field.value}
                label="Gender"
                onChange={field.onChange}
              >
                <MenuItem value={"Female"}>Female</MenuItem>
                <MenuItem value={"Male"}>Male</MenuItem>
              </Select>
            </FormControl>
          )}
        />
        <Controller
          name={"birthDate"}
          control={control}
          rules={{
            required: { value: !!user.birthDate, message: "Required" },
          }}
          render={({ field }) => (
            <DatePicker
              maxDate={new Date()}
              label={"Date of birth"}
              mask={"__.__.____"}
              renderInput={(params) => (
                <ContrastTextField
                  InputLabelProps={{
                    shrink: true,
                  }}
                  {...params}
                  error={!!errors.birthDate && !!errors.birthDate.message}
                  helperText={errors.birthDate?.message}
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
