import React, {useEffect, useState} from "react";
import {Button, Grid, Paper, Stack, Typography, Autocomplete} from "@mui/material";
import ContrastTextField from "../../shared/components/ContrastTextField/ContrastTextField";
import {useNotifier} from "../../shared/hooks/Notifier.hook";
import {SubmitHandler, useForm} from "react-hook-form";
import {useNavigate} from "react-router-dom";
import {CreateTransactionDto} from "../types/CreateTransactionDto"
import {useTransactionService} from "../TransactionService.hook";
import {useUserService} from "../../users/UserService.hook"
import {User} from "../../users/types/User"
import {
    DefaultPaginatedData,
    PaginatedData,
} from "../../shared/types/PaginatedData";


type Inputs = {
    userName: string;
    amount: number;
    description: string;
};

export default function CreateTransactionPage() {
    const transactionService = useTransactionService();
    const userService = useUserService();
    const [users, setUsers] = useState<{ label: string; id: number; }[]>([])
    const [userOptions, setUserOptions] = useState<{ label: string; id: number; }[]>([])
    const [value, setValue] = useState<string| null>('')
    const [inputValue, setInputValue] = useState('');
    const [searchLoading, setSearchLoading] = useState(false);
    const {
        register,
        handleSubmit,
        formState: {errors},
    } = useForm<Inputs>();


    const {showError, showSuccess} = useNotifier();
    const navigate = useNavigate();

    const onSubmit: SubmitHandler<Inputs> = async (data) => {
        try {
            // @ts-ignore
            const dto: CreateTransactionDto = {
                userId: users.filter(a => a.label === data.userName)[0].id,
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
    useEffect(() => {
        (async () => {
            setSearchLoading(true);
            try {
                const items = await userService.getUsers({
                });
                const users = await userService.getUsers({
                    page: {pageNumber: 1, numberOfElementsOnPage: items.totalItems}
                });
                const res = users.data.map(a => ({
                    label: a.email, id: a.id
                }))
                const searchUsersAwait = await userService.getUsers({
                    filters: [{columnName: "email", operator: "cnt", value: value}],
/*
                    page: {pageNumber: 1, numberOfElementsOnPage: 5}
*/
                });
                const searchUserRes = searchUsersAwait.data.map(a => ({
                    label: a.email, id: a.id
                }))
                setUsers(res)
                setUserOptions(searchUserRes);
            } catch (e) {
                showError(e);
            }
            setSearchLoading(false);
            console.log(value, userOptions)
        })();
    }, [value]);
    
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
                                <Autocomplete
                                    autoComplete
                                    onInputChange={(event) => {
                                        setValue((event.target as HTMLInputElement).value)
                                    }}
                                    fullWidth 
                                    isOptionEqualToValue={(option, value) => option.id === value.id}
                                    options={userOptions}
                                    renderInput={(params) =>
                                        (<ContrastTextField
                                            error={!!errors.userName && !!errors.userName.message}
                                            helperText={errors.userName?.message}
                                            label={"User"}
                                            {...params}
                                            {...register("userName", {
                                                required: {value: true, message: "User required"},
                                            })}
                                        />)
                                    }
                                />


                                <ContrastTextField
                                    error={!!errors.amount && !!errors.amount.message}
                                    helperText={errors.amount?.message}
                                    label={"Amount"}
                                    fullWidth
                                    type={"number"}
                                    {...register("amount", {
                                        required: {
                                            value: true,
                                            message: "Amount required",
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
    )
        ;
}
