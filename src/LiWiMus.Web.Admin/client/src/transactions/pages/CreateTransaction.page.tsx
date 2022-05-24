import React, {useState} from "react";
import {Button, Grid, IconButton, InputAdornment, Paper, Stack, Typography, Input, Autocomplete} from "@mui/material";
import ContrastTextField from "../../shared/components/ContrastTextField/ContrastTextField";
import {useNotifier} from "../../shared/hooks/Notifier.hook";
import {SubmitHandler, useForm} from "react-hook-form";
import TransactionService from "../Transaction.service";
import {CreateTransactionDto} from "../types/CreateTransactionDto";
import {useNavigate} from "react-router-dom";
import {Visibility, VisibilityOff} from "@mui/icons-material";

type Inputs = {
    userId: number,
    amount: number,
    description: string
};

export default function CreateTransactionPage() {
    const {
        register,
        handleSubmit,
        formState: {errors},
        setValue,
        clearErrors,
    } = useForm<Inputs>();

    const {showError, showSuccess} = useNotifier();
    const navigate = useNavigate();

    const onSubmit: SubmitHandler<Inputs> = async (data) => {
        try {
            // @ts-ignore
            const dto: CreateUserDto = {
                userId: data.userId,
                amount: data.amount,
                description: data.description
            };
            const transaction = await TransactionService.save(dto);
            showSuccess("Transaction created");
            navigate(`/admin/transactions/${transaction.id}`);
        } catch (e) {
            showError(e);
        }
    };

    return (
        <Grid container spacing={2} justifyContent={"center"}>
            <Grid item xs={12} md={10} lg={8}>
                <Paper sx={{p: 4}} elevation={10}>
                    <Typography variant={"h3"} component={"div"} sx={{mb: 4}}>
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
                                        required: {value: true, message: "User required"},
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
                                     pattern: {value: /^[0-9]+.?[0-9]*$/, message: "Must be a number, use dot as separator"}
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
                                    sx={{borderRadius: "20px", px: 4, width: "200px"}}
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
