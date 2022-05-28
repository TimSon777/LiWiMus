import React, { useEffect, useState } from "react";
import { UserPlan } from "../../types/UserPlan";
import { Action } from "../../../shared/types/Action";
import { useNotifier } from "../../../shared/hooks/Notifier.hook";
import { Controller, SubmitHandler, useForm } from "react-hook-form";
import { UpdateUserPlanDto } from "../../types/UpdateUserPlanDto";
import UserPlanService from "../../UserPlan.service";
import { Button, Link, Stack } from "@mui/material";
import ContrastTextField from "../../../shared/components/ContrastTextField/ContrastTextField";
import { DateTimePicker } from "@mui/x-date-pickers";
import { format } from "date-fns";
import ReadonlyInfo from "../../../shared/components/InfoItem/ReadonlyInfo";
import { Link as RouterLink } from "react-router-dom";

type Props = {
  userPlan: UserPlan;
  setUserPlan: Action<UserPlan>;
};

type Inputs = {
  end: Date;
};

export default function UserPlanInfoEditor({ userPlan, setUserPlan }: Props) {
  const { showSuccess, showError } = useNotifier();
  const [defaultInputs, setDefaultInputs] = useState<Inputs>();
  const {
    handleSubmit,
    watch,
    formState: { errors },
    reset,
    control,
  } = useForm<Inputs>();

  useEffect(() => {
    let inputs = { end: userPlan.end };
    setDefaultInputs(inputs);
    reset(inputs);
  }, [userPlan]);

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
      const req: UpdateUserPlanDto = {
        end: data.end,
      };
      const response = await UserPlanService.update(userPlan.id, req);
      showSuccess("Info updated");
      setUserPlan({ ...userPlan, ...response });
    } catch (error) {
      // @ts-ignore
      showError(error);
    }
  };

  return (
    <form>
      <Stack spacing={3}>
        <ReadonlyInfo
          name={"User"}
          value={
            <Link
              component={RouterLink}
              to={`/admin/users/${userPlan.userId}`}
              underline="none"
              color={"secondary"}
            >
              {userPlan.userName}
            </Link>
          }
        />

        <ReadonlyInfo
          name={"Plan"}
          value={
            <Link
              component={RouterLink}
              to={`/admin/plans/${userPlan.planId}`}
              underline="none"
              color={"secondary"}
            >
              {userPlan.planName}
            </Link>
          }
        />

        <ReadonlyInfo
          name={"Start"}
          value={format(userPlan.start, "dd.MM.yyyy HH:mm")}
        />

        {userPlan.updatable ? (
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
                minDateTime={new Date()}
                InputProps={{ error: !!errors.end }}
                label={"End"}
                mask={"__.__._____"}
                renderInput={(params) => (
                  <ContrastTextField
                    {...params}
                    helperText={errors.end?.message}
                  />
                )}
                {...field}
              />
            )}
          />
        ) : (
          <ReadonlyInfo
            name={"End"}
            value={format(userPlan.end, "dd.MM.yyyy HH:mm")}
          />
        )}

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
