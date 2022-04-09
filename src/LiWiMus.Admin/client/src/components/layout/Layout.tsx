import React from "react";
import NavMenu from "../navmenu/NavMenu";
import { Container } from "@mui/material";

export interface LayoutComponentProps {
  children: React.ReactNode;
}

export function Layout({ children }: LayoutComponentProps) {
  return (
    <>
      <NavMenu />
      <Container
        maxWidth={false}
        sx={{
          height: "calc(100% - 69.5px)",
          bgcolor: "background.default",
          pt: 1,
        }}
      >
        {children}
      </Container>
    </>
  );
}
