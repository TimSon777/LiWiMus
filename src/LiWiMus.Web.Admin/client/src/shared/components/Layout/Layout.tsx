import React, { PropsWithChildren, useContext } from "react";
import NavMenu from "../NavMenu/NavMenu";
import CssBaseline from "@mui/material/CssBaseline";
import Box from "@mui/material/Box";
import SideBar from "../SideBar/SideBar";
import { AuthContext } from "../../contexts/Auth.context";

export function Layout(props: PropsWithChildren<any>) {
  const [mobileOpen, setMobileOpen] = React.useState(false);
  const { isAuthenticated } = useContext(AuthContext);

  return (
    <>
      <CssBaseline />
      <NavMenu mobileOpen={mobileOpen} setMobileOpen={setMobileOpen} />
      <Box
        sx={{
          display: "flex",
          pt: "64px",
          height: "100%",
        }}
      >
        {isAuthenticated && (
          <SideBar mobileOpen={mobileOpen} setMobileOpen={setMobileOpen} />
        )}
        <Box sx={{ px: { md: 7, xs: 2, sm: 4 }, py: 2, width: "100%" }}>
          {props.children}
        </Box>
      </Box>
    </>
  );
}
