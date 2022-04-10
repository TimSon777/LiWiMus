import React from "react";
import { Link as RLink } from "react-router-dom";
import logo from "../../images/logo.jpg";
import { Avatar, Link } from "@mui/material";
import "../../App.sass";
import AppBar from "@mui/material/AppBar";
import Toolbar from "@mui/material/Toolbar";

export default function NavMenu() {
  return (
    <AppBar
      position="fixed"
      sx={{
        zIndex: (theme) => theme.zIndex.drawer + 1,
        bgcolor: "background.paperLight",
      }}
    >
      <Toolbar>
        <Link
          color="primary.contrastText"
          component={RLink}
          to={"/admin"}
          underline="none"
          sx={{
            display: "inline-flex",
            alignItems: "center",
            fontSize: "h4.fontSize",
          }}
        >
          <Avatar alt="Logo" src={logo} sx={{ width: 50, height: 50, mr: 2 }} />
          <span>LiWiMus</span>
        </Link>
      </Toolbar>
    </AppBar>
  );
}
