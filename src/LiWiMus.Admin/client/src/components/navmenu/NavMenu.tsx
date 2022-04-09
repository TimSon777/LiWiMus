import React from "react";
import { Link as RLink } from "react-router-dom";
import logo from "../../images/logo.jpg";
import styles from "./NavMenu.module.sass";
import "../../App.sass";
import { Avatar, Container, Link } from "@mui/material";

export default function NavMenu() {
  return (
    <Container maxWidth={false} sx={{ bgcolor: "background.paper", p: 1 }}>
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
        <span className={styles.overpass}>LiWiMus</span>
      </Link>
    </Container>
  );
}
