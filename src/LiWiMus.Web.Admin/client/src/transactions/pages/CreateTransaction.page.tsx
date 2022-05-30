import React from "react";
import { Button, Grid, Paper, Stack, Typography } from "@mui/material";
import ContrastTextField from "../../shared/components/ContrastTextField/ContrastTextField";
import { useNotifier } from "../../shared/hooks/Notifier.hook";
import { SubmitHandler, useForm } from "react-hook-form";
import { useNavigate } from "react-router-dom";
import { useTransactionService } from "../TransactionService.hook";

type Inputs = {
  userId: number;
  amount: number;
  description: string;
};

export default function CreateTransactionPage() {
  const transactionService = useTransactionService();

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
      const dto: CreateUserDto = {
        userId: data.userId,
        amount: data.amount,
        description: data.description,
      };
      const transaction = await transactionService.save(dto);
      showSuccess("Transaction created");
      navigate(`/admin/transactions/${transaction.id}`);
    } catch (e) {
      showError(e);
    }
  };

  return (
    <Grid container spacing={2} justifyContent={"center"}>
      <Grid item xs={12} md={10} lg={8}>
        <Paper sx={{ p: 4 }} elevation={10}>
          <Typography variant={"h3"} component={"div"} sx={{ mb: 4 }}>
            New transaction
          </Typography>

          <Grid>
            <form onSubmit={handleSubmit(onSubmit)}>
              <Stack spacing={2} alignItems={"center"}>
                <ContrastTextField
                  error={!!errors.userId && !!errors.userId.message}
                  helperText={errors.userId?.message}
                  label={"UserId"}
                  fullWidth
                  {...register("userId", {
                    required: { value: true, message: "User required" },
                  })}
                />

                <ContrastTextField
                  error={!!errors.amount && !!errors.amount.message}
                  helperText={errors.amount?.message}
                  label={"Amount"}
                  fullWidth
                  multiline
                  {...register("amount", {
                    required: {
                      value: true,
                      message: "Amount required",
                    },
                    pattern: {
                      value: /^[0-9]+.?[0-9]*$/,
                      message: "Must be a number, use dot as separator",
                    },
                  })}
                />
                <ContrastTextField
                  error={!!errors.description && !!errors.description.message}
                  label="Description"
                  variant="outlined"
                  fullWidth
                  {...register("description", {
                    required: {
                      value: true,
                      message: "Description required",
                    },
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
