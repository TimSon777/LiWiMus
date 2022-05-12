import React, { PropsWithChildren, useContext } from "react";
import NavMenu from "../NavMenu/NavMenu";
import CssBaseline from "@mui/material/CssBaseline";
import Box from "@mui/material/Box";
import SideBar from "../SideBar/SideBar";
import { AuthContext } from "../../contexts/Auth.context";
import { UserData } from "../../types/UserData";

export function Layout(
  props: PropsWithChildren<{ userData: UserData | null }>
) {
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
          <SideBar
            userData={props.userData}
            mobileOpen={mobileOpen}
            setMobileOpen={setMobileOpen}
          />
        )}
        <Box sx={{ m: 3, width: "100%" }}>{props.children}</Box>
      </Box>
    </>
  );
}
