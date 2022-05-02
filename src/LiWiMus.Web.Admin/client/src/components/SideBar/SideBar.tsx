import React, { useContext } from "react";
import {
  Avatar,
  Box,
  Divider,
  Drawer,
  List,
  Toolbar,
  Typography,
} from "@mui/material";
import { AuthContext } from "../../contexts/Auth.context";
import GroupIcon from "@mui/icons-material/Group";
import DashboardIcon from "@mui/icons-material/Dashboard";
import LogoutIcon from "@mui/icons-material/Logout";
import ListLink from "../ListLink/ListLink";
import ListButton from "../ListButton/ListButton";
import "./SideBar.sass";
import { UserData } from "../../types/UserData";

const drawerWidth = 240;

export interface SideBarProps {
  userData: UserData | null;
  mobileOpen: boolean;
  setMobileOpen: (state: boolean) => void;
}

export default function SideBar({ mobileOpen, setMobileOpen, userData }: SideBarProps) {
  const { logout } = useContext(AuthContext);

  const linkClickHandler = () => {
    setMobileOpen(false);
  };

  const drawer = (
    <>
      <Toolbar />
      <Box sx={{ overflow: "auto" }}>
        <Box sx={{ m: 2, display: "inline-flex", alignItems: "center" }}>
          <Avatar sx={{ mr: 2 }} />
          <Typography noWrap>{userData?.name || "anonym"}</Typography>
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
        <List>
          <ListButton
            icon={<LogoutIcon />}
            text={"Logout"}
            onClick={() => {
              linkClickHandler();
              logout();
            }}
          />
        </List>
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
        onClose={() => setMobileOpen(!mobileOpen)}
        ModalProps={{
          keepMounted: true,
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
