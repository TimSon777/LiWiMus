import React from "react";
import {Role} from "../../types/Role";
import {Button, Stack} from "@mui/material";
import {useNotifier} from "../../../shared/hooks/Notifier.hook";
import {Controller, SubmitHandler, useForm} from "react-hook-form";
import {UpdateRoleDto} from "../../types/UpdateRoleDto";
import RoleService from "../../Role.service";
import ContrastTextField from "../../../shared/components/ContrastTextField/ContrastTextField";

type Props = {
  role: Role;
  setRole: (role: Role) => void;
};

type Inputs = {
  description: string;
};

export default function RoleInfoEditor({ role, setRole }: Props) {
  const defaultInputs: Inputs = {
    description: role.description,
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
  const isChanged =
    JSON.stringify({
      description: actual.description,
    }) !==
    JSON.stringify({
      description: defaultInputs.description,
    });

  const rollbackHandler = () => {
    reset(defaultInputs);
  };

  const saveHandler: SubmitHandler<Inputs> = async (data) => {
    if (!isChanged) {
      return;
    }
    try {
      const req: UpdateRoleDto = {
        id: +role.id,
        description: data.description,
      };
      const response = await RoleService.update(req);
      showSuccess("Info updated");
      setRole({ ...role, ...response });
    } catch (error) {
      // @ts-ignore
      showError(error);
    }
  };

  return (
    <form>
      <Stack spacing={3}>
        <Controller
          name="description"
          control={control}
          rules={{
            required: true,
            maxLength: { value: 500, message: "Max length - 500" },
          }}
          render={({ field }) => (
            <ContrastTextField
              error={!!errors.description && !!errors.description.message}
              helperText={errors.description?.message}
              label={"Description"}
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
    </form>
  );
}