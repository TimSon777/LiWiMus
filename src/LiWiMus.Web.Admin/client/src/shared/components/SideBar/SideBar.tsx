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
import ReceiptIcon from '@mui/icons-material/Receipt';
import DashboardIcon from "@mui/icons-material/Dashboard";
import AudiotrackIcon from '@mui/icons-material/Audiotrack';
import QueueMusicIcon from '@mui/icons-material/QueueMusic';
import TheaterComedyIcon from '@mui/icons-material/TheaterComedy';
import MonetizationOnIcon from '@mui/icons-material/MonetizationOn';
import LogoutIcon from "@mui/icons-material/Logout";
import ListLink from "../ListLink/ListLink";
import ListButton from "../ListButton/ListButton"
import AlbumIcon from '@mui/icons-material/Album';
import "./SideBar.sass";
import { UserData } from "../../types/UserData";

const drawerWidth = 240;

export interface SideBarProps {
  userData: UserData | null;
  mobileOpen: boolean;
  setMobileOpen: (state: boolean) => void;
}

export default function SideBar({
  mobileOpen,
  setMobileOpen,
  userData,
}: SideBarProps) {
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
          <ListLink
            icon={<AlbumIcon/>}
            text={"Albums"}
            to={"/admin/albums"}
            onClick={linkClickHandler}
            />
          <ListLink
              icon={<QueueMusicIcon />}
              text={"Playlists"}
              to={"/admin/playlists"}
              onClick={linkClickHandler}
          />
          <ListLink
            icon={<AudiotrackIcon />}
            text={"Tracks"}
            to={"/admin/tracks"}
            onClick={linkClickHandler}
            />
          <ListLink
              icon={<TheaterComedyIcon />}
              text={"Genres"}
              to={"/admin/genres"}
              onClick={linkClickHandler}
          />
          <ListLink
              icon={<ReceiptIcon />}
              text={"Transactions"}
              to={"/admin/transactions"}
              onClick={linkClickHandler}
          />
          <ListLink
              icon={<MonetizationOnIcon />}
              text={"Plans"}
              to={"/admin/plans"}
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
