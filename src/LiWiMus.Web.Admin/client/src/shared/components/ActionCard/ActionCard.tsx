import React, { ReactElement } from "react";
import { Box, Paper, Typography } from "@mui/material";
import styles from "./ActionCard.module.sass";

type Props = {
  text: string;
  action: ReactElement;
};

export default function ActionCard({ text, action }: Props) {
  return (
    <Paper
      elevation={10}
      className={styles.container}
      sx={{ width: "100%", pb: "100%", position: "relative" }}
    >
      <Box className={styles.content}>
        <Typography>{text}</Typography>
      </Box>
      <Box className={styles.actions}>{action}</Box>
    </Paper>
  );
}
