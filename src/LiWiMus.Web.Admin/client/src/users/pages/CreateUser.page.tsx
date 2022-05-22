import React, {useState} from "react";
import {Button, Grid, IconButton, InputAdornment, Paper, Stack, Typography} from "@mui/material";
import ContrastTextField from "../../shared/components/ContrastTextField/ContrastTextField";
import {useNotifier} from "../../shared/hooks/Notifier.hook";
import {SubmitHandler, useForm} from "react-hook-form";
import UserService from "../User.service";
import {CreateUserDto} from "../types/CreateUserDto";
import {useNavigate} from "react-router-dom";
import {Visibility, VisibilityOff} from "@mui/icons-material";

type Inputs = {
    userName: string;
    email: string;
    password: string;
};

export default function CreateUserPage() {
    const {
        register,
        handleSubmit,
        formState: {errors},
        setValue,
        clearErrors,
    } = useForm<Inputs>();
    const {showError, showSuccess} = useNotifier();
    const [showPassword, setShowPassword] = useState(false);
    const navigate = useNavigate();

    const onSubmit: SubmitHandler<Inputs> = async (data) => {
        try {
            // @ts-ignore
            const dto: CreateUserDto = {
                userName: data.userName,
                email: data.email,
                password: data.password
            };
            const user = await UserService.save(dto);
            showSuccess("User created");
            navigate(`/admin/users/${user.id}`);
        } catch (e) {
            showError(e);
        }
    };

    return (
        <Grid container spacing={2} justifyContent={"center"}>
            <Grid item xs={12} md={10} lg={8}>
                <Paper sx={{p: 4}} elevation={10}>
                    <Typography variant={"h3"} component={"div"} sx={{mb: 4}}>
                        New user
                    </Typography>

                    <Grid>
                        <form onSubmit={handleSubmit(onSubmit)}>
                            <Stack spacing={2} alignItems={"center"}>
                                <ContrastTextField
                                    error={!!errors.userName && !!errors.userName.message}
                                    helperText={errors.userName?.message}
                                    label={"Username"}
                                    fullWidth
                                    {...register("userName", {
                                        required: {value: true, message: "Username required"},
                                        minLength: {value: 5, message: "Min length - 5"},
                                        maxLength: {value: 50, message: "Max length - 50"},
                                    })}
                                />
                                <ContrastTextField
                                    error={!!errors.email && !!errors.email.message}
                                    helperText={errors.email?.message}
                                    label={"Email"}
                                    fullWidth
                                    multiline
                                    {...register("email", {
                                        required: {
                                            value: true,
                                            message: "Email required",
                                        },
                                        maxLength: {value: 500, message: "Max length - 500"},
                                        pattern: {
                                            value: /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/,
                                            message: "Enter valid email"
                                        },
                                    })}
                                />
                                <ContrastTextField
                                    error={!!errors.password && !!errors.password.message}
                                    label="Password"
                                    variant="outlined"
                                    fullWidth
                                    type={showPassword ? "text" : "password"}
                                    {...register("password", {
                                        required: {value: true, message: "Enter password"},
                                        pattern: {
                                            value: /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/,
                                            message: "Minimum eight characters, at least one uppercase letter, one lowercase letter, one number and one special character"
                                        }
                                    })}
                                    helperText={errors.password?.message}
                                    InputProps={{
                                        endAdornment: (
                                            <InputAdornment position="end">
                                                <IconButton
                                                    aria-label="toggle password visibility"
                                                    onClick={() => setShowPassword(!showPassword)}
                                                    onMouseDown={(e) => e.preventDefault()}
                                                    edge="end"
                                                >
                                                    {showPassword ? <VisibilityOff/> : <Visibility/>}
                                                </IconButton>
                                            </InputAdornment>
                                        ),
                                    }}
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
