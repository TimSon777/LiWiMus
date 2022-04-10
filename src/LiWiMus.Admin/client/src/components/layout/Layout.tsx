import React from "react";
import NavMenu from "../navmenu/NavMenu";
import CssBaseline from "@mui/material/CssBaseline";
import Box from "@mui/material/Box";
import SideBar from "../SideBar/Drawer";

export interface LayoutComponentProps {
  children: React.ReactNode;
  isAuthenticated: boolean;
}

export function Layout(props: LayoutComponentProps) {
  const drawer = props.isAuthenticated ? <SideBar /> : "";

  return (
    <>
      <CssBaseline />
      <NavMenu />
      <Box
        sx={{
          display: "flex",
          pt: "64px",
          height: "100%",
        }}
      >
        {drawer}
        <Box sx={{ m: 3 }}>{props.children}</Box>
      </Box>
    </>
  );
}
