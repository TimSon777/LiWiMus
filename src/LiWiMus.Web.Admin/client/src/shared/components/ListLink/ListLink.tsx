import React, { cloneElement } from "react";
import {
  ListItem,
  ListItemIcon,
  ListItemText,
  Typography,
} from "@mui/material";
import { useMatch, useNavigate, useResolvedPath } from "react-router-dom";

export interface ListLinkProps {
  icon?: JSX.Element;
  to: string;
  text: string;
  onClick?: () => void;
}

export default function ListLink({ icon, onClick, text, to }: ListLinkProps) {
  let resolved = useResolvedPath(to);
  let match = useMatch({ path: resolved.pathname, end: true });

  const navigate = useNavigate();

  const clickHandler = () => {
    navigate(to);
    if (onClick) {
      onClick();
    }
  };

  const styles = match ? { color: "primary" } : {};

  const listItemIcon = icon ? (
    <ListItemIcon>{cloneElement(icon, styles)}</ListItemIcon>
  ) : (
    ""
  );

  return (
    <ListItem button onClick={clickHandler}>
      {listItemIcon}
      <ListItemText>
        <Typography {...styles}>{text}</Typography>
      </ListItemText>
    </ListItem>
  );
}
