import React from "react";
import NavMenu from "../NavMenu/NavMenu";
import CssBaseline from "@mui/material/CssBaseline";
import Box from "@mui/material/Box";
import SideBar from "../SideBar/SideBar";

export interface LayoutComponentProps {
  children: React.ReactNode;
  isAuthenticated: boolean;
}

export function Layout(props: LayoutComponentProps) {
  const [mobileOpen, setMobileOpen] = React.useState(false);
  const handleDrawerToggle = () => {
    setMobileOpen(!mobileOpen);
  };

  const drawer = props.isAuthenticated ? (
    <SideBar handleDrawerToggle={handleDrawerToggle} mobileOpen={mobileOpen} />
  ) : (
    ""
  );

  return (
    <>
      <CssBaseline />
      <NavMenu handleDrawerToggle={handleDrawerToggle} />
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
