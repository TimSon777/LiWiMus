import React from "react";
import { Link as RLink } from "react-router-dom";
import { Avatar, Link } from "@mui/material";
import AppBar from "@mui/material/AppBar";
import Toolbar from "@mui/material/Toolbar";
import IconButton from "@mui/material/IconButton";
import MenuIcon from "@mui/icons-material/Menu";
import logo from "../../images/logo.jpg";
import "../../App.sass";

export interface NavMenuProps {
  handleDrawerToggle: () => void;
  isAuthenticated: boolean;
}

export default function NavMenu({
  handleDrawerToggle,
  isAuthenticated,
}: NavMenuProps) {
  return (
    <AppBar
      position="fixed"
      sx={{
        zIndex: (theme) => theme.zIndex.drawer + 1,
        bgcolor: "background.paperLight",
      }}
    >
      <Toolbar>
        {isAuthenticated && (
          <IconButton
            color="inherit"
            aria-label="open drawer"
            edge="start"
            onClick={handleDrawerToggle}
            sx={{ mr: 2, display: { sm: "none" } }}
          >
            <MenuIcon />
          </IconButton>
        )}

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
