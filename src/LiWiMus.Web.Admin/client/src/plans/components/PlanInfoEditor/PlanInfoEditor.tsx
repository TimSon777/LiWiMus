import React from "react";
import { Plan } from "../../types/Plan";
import { Button, Stack } from "@mui/material";
import { useNotifier } from "../../../shared/hooks/Notifier.hook";
import { Controller, SubmitHandler, useForm } from "react-hook-form";
import { UpdatePlanDto } from "../../types/UpdatePlanDto";
import PlanService from "../../Plan.service";
import ContrastTextField from "../../../shared/components/ContrastTextField/ContrastTextField";

type Props = {
  plan: Plan;
  setPlan: (plan: Plan) => void;
};

type Inputs = {
  description: string;
  pricePerMonth: number;
};

export default function PlanInfoEditor({ plan, setPlan }: Props) {
  const defaultInputs: Inputs = {
    description: plan.description,
    pricePerMonth: plan.pricePerMonth,
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
      price: (+actual.pricePerMonth).toFixed(2),
    }) !==
    JSON.stringify({
      description: defaultInputs.description,
      price: (+defaultInputs.pricePerMonth).toFixed(2),
    });

  const rollbackHandler = () => {
    reset(defaultInputs);
  };

  const saveHandler: SubmitHandler<Inputs> = async (data) => {
    if (!isChanged) {
      return;
    }
    try {
      const req: UpdatePlanDto = {
        id: +plan.id,
        description: data.description,
        pricePerMonth: data.pricePerMonth,
      };
      const response = await PlanService.update(req);
      showSuccess("Info updated");
      setPlan({ ...plan, ...response });
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

        <Controller
          name={"pricePerMonth"}
          control={control}
          rules={{
            validate: (value) => (value ? true : "Not a number"),
          }}
          render={({ field }) => (
            <ContrastTextField
              error={!!errors.pricePerMonth && !!errors.pricePerMonth.message}
              helperText={errors.pricePerMonth?.message}
              label="Price per month"
              InputLabelProps={{
                shrink: true,
              }}
              variant="outlined"
              fullWidth
              type={"number"}
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
