import React, { cloneElement } from "react";
import {
  ListItem,
  ListItemIcon,
  ListItemText,
  Typography,
} from "@mui/material";
import { useMatch, useNavigate, useResolvedPath } from "react-router-dom";

export interface ListLinkProps {
  icon: JSX.Element;
  to: string;
  text: string;
}

export default function ListLink(props: ListLinkProps) {
  let resolved = useResolvedPath(props.to);
  let match = useMatch({ path: resolved.pathname, end: true });

  const navigate = useNavigate();

  const styles = match ? { color: "primary" } : {};

  return (
    <ListItem button onClick={() => navigate(props.to)}>
      <ListItemIcon>{cloneElement(props.icon, styles)}</ListItemIcon>
      <ListItemText>
        <Typography {...styles}>{props.text}</Typography>
      </ListItemText>
    </ListItem>
  );
}
