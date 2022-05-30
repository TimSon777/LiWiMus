import React from "react";
import { useNotifier } from "../../../shared/hooks/Notifier.hook";
import { SubmitHandler, useForm } from "react-hook-form";
import { Box, Button, Stack } from "@mui/material";
import ContrastTextField from "../../../shared/components/ContrastTextField/ContrastTextField";
import { useTransactionService } from "../../TransactionService.hook";

type Inputs = {
  description: string;
};

type Props = {
  id: string;
  dto: Inputs;
  setDto: (dto: Inputs) => void;
};

export default function TransactionInfoEditor({ id, dto, setDto }: Props) {
  const transactionService = useTransactionService();

  const { showSuccess, showError } = useNotifier();

  const {
    register,
    handleSubmit,
    watch,
    formState: { errors },
    reset,
  } = useForm<Inputs>({ defaultValues: dto });

  const actual = watch();
  const isChanged = JSON.stringify(actual) !== JSON.stringify(dto);

  const rollbackHandler = () => {
    reset(dto);
  };

  const saveHandler: SubmitHandler<Inputs> = async (data) => {
    if (JSON.stringify(data) === JSON.stringify(dto)) {
      return;
    }
    try {
      const req = { ...data, id: +id };
      const response = (await transactionService.update(req)) as Inputs;
      showSuccess("Info updated");
      setDto(response);
    } catch (error) {
      // @ts-ignore
      showError(error);
    }
  };

  return (
    <Box sx={{ display: "flex", flexDirection: "column", width: "100%" }}>
      <Stack direction={"column"} spacing={2}>
        <ContrastTextField
          error={!!errors.description && !!errors.description.message}
          helperText={errors.description?.message}
          label="Description"
          InputLabelProps={{
            shrink: true,
          }}
          variant="outlined"
          fullWidth
          {...register("description", {
            required: { value: true, message: "Enter transaction description" },
            maxLength: {
              value: 100,
              message: "Maximum length - 100 characters",
            },
          })}
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
    </Box>
  );
}
