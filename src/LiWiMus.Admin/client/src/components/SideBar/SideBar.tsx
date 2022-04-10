import React from "react";
import {
  Avatar,
  Box,
  Divider,
  Drawer,
  List,
  Toolbar,
  Typography,
} from "@mui/material";
import GroupIcon from "@mui/icons-material/Group";
import DashboardIcon from "@mui/icons-material/Dashboard";
import ListLink from "../ListLink/ListLink";
import "./SideBar.sass";

const drawerWidth = 240;

export interface SideBarProps {
  mobileOpen: boolean;
  handleDrawerToggle: () => void;
}

export default function SideBar({
  mobileOpen,
  handleDrawerToggle,
}: SideBarProps) {
  const linkClickHandler = () => {
    if (mobileOpen) {
      handleDrawerToggle();
    }
  };

  const drawer = (
    <>
      <Toolbar />
      <Box sx={{ overflow: "auto" }}>
        <Box sx={{ m: 2, display: "inline-flex", alignItems: "center" }}>
          <Avatar sx={{ mr: 2 }} />
          <Typography noWrap>User name</Typography>
        </Box>
        <Divider />
        <List>
          <ListLink
            icon={<DashboardIcon />}
            text={"Dashboard"}
            to={"/admin/dashboard"}
            onClick={linkClickHandler}
          />

          <ListLink
            icon={<GroupIcon />}
            text={"Users"}
            to={"/admin/users"}
            onClick={linkClickHandler}
          />

          <ListLink
            icon={<GroupIcon />}
            text={"Artists"}
            to={"/admin/artists"}
            onClick={linkClickHandler}
          />
        </List>
        <Divider />
      </Box>
    </>
  );

  return (
    <Box
      component="nav"
      sx={{ width: { sm: drawerWidth }, flexShrink: { sm: 0 } }}
      aria-label="mailbox folders"
    >
      <Drawer
        variant="temporary"
        open={mobileOpen}
        onClose={handleDrawerToggle}
        ModalProps={{
          keepMounted: true, // Better open performance on mobile.
        }}
        sx={{
          display: { xs: "block", sm: "none" },
          "& .MuiDrawer-paper": {
            boxSizing: "border-box",
            width: drawerWidth,
          },
        }}
      >
        {drawer}
      </Drawer>
      <Drawer
        variant="permanent"
        sx={{
          display: { xs: "none", sm: "block" },
          "& .MuiDrawer-paper": {
            boxSizing: "border-box",
            width: drawerWidth,
          },
        }}
        open
      >
        {drawer}
      </Drawer>
    </Box>
  );
}
