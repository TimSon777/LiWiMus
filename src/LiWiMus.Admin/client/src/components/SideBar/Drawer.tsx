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

const drawerWidth = 240;

export default function SideBar() {
  return (
    <Drawer
      variant="permanent"
      sx={{
        width: drawerWidth,
        flexShrink: 0,
        [`& .MuiDrawer-paper`]: {
          width: drawerWidth,
          boxSizing: "border-box",
        },
      }}
    >
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
          />

          <ListLink icon={<GroupIcon />} text={"Users"} to={"/admin/users"} />

          <ListLink
            icon={<GroupIcon />}
            text={"Artists"}
            to={"/admin/artists"}
          />
        </List>
        <Divider />
      </Box>
    </Drawer>
  );
}
