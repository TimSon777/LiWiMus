import React from "react";
import { Button, Grid, Paper, Stack, Typography } from "@mui/material";
import ContrastTextField from "../../shared/components/ContrastTextField/ContrastTextField";
import { useNotifier } from "../../shared/hooks/Notifier.hook";
import { SubmitHandler, useForm } from "react-hook-form";
import { CreatePlanDto } from "../types/CreatePlanDto";
import { useNavigate } from "react-router-dom";
import { usePlanService } from "../PlanService.hook";

type Inputs = {
  name: string;
  pricePerMonth: number;
  description: string;
};

export default function CreatePlanPage() {
  const PlanService = usePlanService();

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
            const dto: CreatePlanDto = {
                name: data.name,
                description: data.description,
                pricePerMonth: data.pricePerMonth
            };
            const plan = await PlanService.save(dto);
            showSuccess("Plan created");
            navigate(`/admin/plans/${plan.id}`);
        } catch (e) {
            showError(e);
        }
    };

  return (
    <Grid container spacing={2} justifyContent={"center"}>
      <Grid item xs={12} md={10} lg={8}>
        <Paper sx={{ p: 4 }} elevation={10}>
          <Typography variant={"h3"} component={"div"} sx={{ mb: 4 }}>
            New plan
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
                    minLength: { value: 4, message: "Min length - 4" },
                    maxLength: { value: 50, message: "Max length - 50" },
                  })}
                />
                <ContrastTextField
                  error={!!errors.description && !!errors.description.message}
                  helperText={errors.description?.message}
                  label={"Description"}
                  fullWidth
                  {...register("description", {
                    required: { value: true, message: "Description required" },
                    minLength: { value: 4, message: "Min length - 4" },
                    maxLength: { value: 50, message: "Max length - 50" },
                  })}
                />
                <ContrastTextField
                  error={
                    !!errors.pricePerMonth && !!errors.pricePerMonth.message
                  }
                  helperText={errors.pricePerMonth?.message}
                  label={"Price Per Month"}
                  type={"number"}
                  fullWidth
                  {...register("pricePerMonth", {
                    required: {
                      value: true,
                      message: "Price Per Month required",
                    },
                    min: { value: 0, message: "Positive price" },
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
