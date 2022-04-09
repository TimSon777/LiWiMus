import React from "react";
import { Button, Card, Grid, Stack, TextField } from "@mui/material";
import ArrowForwardIcon from "@mui/icons-material/ArrowForward";

export default function LoginPage() {
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
        >
          Next
        </Button>
      </Grid>
    </Grid>
  );
}
