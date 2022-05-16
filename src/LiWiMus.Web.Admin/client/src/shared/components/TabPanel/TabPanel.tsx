import React from "react";
import { Box } from "@mui/material";
import styles from "./TabPanel.module.sass";

interface TabPanelProps {
  children?: React.ReactNode;
  index: number;
  value: number;
}

export default function TabPanel(props: TabPanelProps) {
  const { children, value, index, ...other } = props;
  const isVisible = value === index;

  return (
    <Box
      className={`${styles.box} ${!isVisible ? styles.hide : ""}`}
      {...other}
    >
      {children}
    </Box>
  );
}
