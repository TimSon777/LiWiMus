import React, { useContext } from "react";
import {
  Avatar,
  Box,
  Divider,
  Drawer,
  Link,
  List,
  Toolbar,
} from "@mui/material";
import { AuthContext } from "../../contexts/Auth.context";
import GroupIcon from "@mui/icons-material/Group";
import ReceiptIcon from "@mui/icons-material/Receipt";
import AudiotrackIcon from "@mui/icons-material/Audiotrack";
import QueueMusicIcon from "@mui/icons-material/QueueMusic";
import TheaterComedyIcon from "@mui/icons-material/TheaterComedy";
import MonetizationOnIcon from "@mui/icons-material/MonetizationOn";
import LogoutIcon from "@mui/icons-material/Logout";
import ListLink from "../ListLink/ListLink";
import ListButton from "../ListButton/ListButton";
import AlbumIcon from "@mui/icons-material/Album";
import "./SideBar.sass";
import { User } from "../../../users/types/User";
import avatarPlaceholder from "../../images/avatar-placeholder.jpg";
import { Link as RLink } from "react-router-dom";
import { useFileService } from "../../hooks/FileService.hook";

const drawerWidth = 240;

export interface SideBarProps {
  user: User | null;
  mobileOpen: boolean;
  setMobileOpen: (state: boolean) => void;
}

export default function SideBar({
  mobileOpen,
  setMobileOpen,
  user,
}: SideBarProps) {
  const fileService = useFileService();

  const { logout } = useContext(AuthContext);

  const linkClickHandler = () => {
    setMobileOpen(false);
  };

  const drawer = (
    <>
      <Toolbar />
      <Box sx={{ overflow: "auto" }}>
        {user && (
          <Box sx={{ m: 2, display: "inline-flex", alignItems: "center" }}>
            <Avatar
              sx={{ mr: 2 }}
              src={
                user.avatarLocation
                  ? fileService.getLocation(user.avatarLocation)
                  : avatarPlaceholder
              }
            />
            <Link
              underline={"none"}
              color={"secondary"}
              component={RLink}
              to={`/admin/users/${user.id}`}
            >
              {user.userName}
            </Link>
          </Box>
        )}
        <Divider />
        <List>
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
            icon={<AlbumIcon />}
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
