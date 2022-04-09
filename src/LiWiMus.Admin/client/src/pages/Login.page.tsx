import React, { useContext } from "react";
import { Button, Grid, Stack, TextField } from "@mui/material";
import ArrowForwardIcon from "@mui/icons-material/ArrowForward";
import { AuthContext } from "../contexts/Auth.context";

export default function LoginPage() {
  const auth = useContext(AuthContext);

  const loginHandler = () => {
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
          <TextField label="Username" variant="outlined" fullWidth />
          <TextField label="Password" variant="outlined" type="password" />
        </Stack>
      </Grid>
      <Grid item xs="auto">
        <Button
          variant="contained"
          endIcon={<ArrowForwardIcon />}
          sx={{ borderRadius: "20px", px: 4 }}
          onClick={loginHandler}
        >
          Next
        </Button>
      </Grid>
    </Grid>
  );
}
