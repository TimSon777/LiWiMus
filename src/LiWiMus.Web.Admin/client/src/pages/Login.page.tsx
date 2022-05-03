import React, { useContext, useState } from "react";
import {
  Box,
  Button,
  Grid,
  IconButton,
  InputAdornment,
  Stack,
} from "@mui/material";
import ArrowForwardIcon from "@mui/icons-material/ArrowForward";
import { AuthContext } from "../contexts/Auth.context";
import { SubmitHandler, useForm } from "react-hook-form";
import ContrastTextField from "../components/ContrastTextField/ContrastTextField";
import { Visibility, VisibilityOff } from "@mui/icons-material";

interface ILoginInput {
  username: string;
  password: string;
}

export default function LoginPage() {
  const [showPassword, setShowPassword] = useState(false);

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
    <Box sx={{ position: "relative", height: "100%" }}>
      <Grid
        container
        direction="row"
        justifyContent="center"
        alignItems="center"
        spacing={2}
        sx={{ position: "absolute", top: "50%", transform: "translateY(-50%)" }}
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
              type={showPassword ? "text" : "password"}
              {...register("password", {
                required: { value: true, message: "Enter password" },
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
                      {showPassword ? <VisibilityOff /> : <Visibility />}
                    </IconButton>
                  </InputAdornment>
                ),
              }}
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
    </Box>
  );
}
