import React, { useContext } from "react";
import { Button, Grid, Stack } from "@mui/material";
import ArrowForwardIcon from "@mui/icons-material/ArrowForward";
import { AuthContext } from "../contexts/Auth.context";
import { SubmitHandler, useForm } from "react-hook-form";
import ContrastTextField from "../components/ContrastTextField/ContrastTextField";

interface ILoginInput {
  username: string;
  password: string;
}

export default function LoginPage() {
  const auth = useContext(AuthContext);

  const {
    register,
    setError,
    formState: { errors },
    handleSubmit,
  } = useForm<ILoginInput>();

  const loginHandler: SubmitHandler<ILoginInput> = async (data) => {
    console.log(data);
    auth.login("token", "id");
  };

  return (
    <Grid
      container
      direction="row"
      justifyContent="center"
      alignItems="center"
      sx={{ height: "100%" }}
      spacing={2}
    >
      <Grid item xs="auto" lg={2}>
        <Stack spacing={2}>
          <ContrastTextField
            error={!!errors.username && !!errors.username.message}
            label="Username"
            variant="outlined"
            fullWidth
            {...register("username", {
              required: { value: true, message: "Enter username" },
            })}
            helperText={errors.username?.message}
          />
          <ContrastTextField
            error={!!errors.password && !!errors.password.message}
            label="Password"
            variant="outlined"
            type="password"
            {...register("password", {
              required: { value: true, message: "Enter password" },
            })}
            helperText={errors.password?.message}
          />
        </Stack>
      </Grid>
      <Grid item xs="auto">
        <Button
          variant="contained"
          endIcon={<ArrowForwardIcon />}
          sx={{ borderRadius: "20px", px: 4 }}
          onClick={handleSubmit(loginHandler)}
        >
          Next
        </Button>
      </Grid>
    </Grid>
  );
}
